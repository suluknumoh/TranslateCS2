using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using Prism.Mvvm;
using Prism.Regions;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Configurations.Views;
using TranslateCS2.Core.Services.Databases;
using TranslateCS2.Core.Services.InstallPaths;
using TranslateCS2.Core.Services.LocalizationFiles;
using TranslateCS2.Inf;
using TranslateCS2.Inf.Models;

namespace TranslateCS2.Core.Sessions;
internal class TranslationSessionManager : BindableBase, ITranslationSessionManager {
    private readonly IRegionManager regionManager;
    private readonly IViewConfigurations viewConfigurations;
    private readonly ITranslationsDatabaseService.OnErrorCallBack onError;
    private readonly ITranslationsDatabaseService db;
    public ILocalizationFile BaseLocalizationFile { get; }
    public InstallPathDetector InstallPathDetector { get; }
    public ILocalizationFileService LocalizationFilesService { get; }
    public IEnumerable<FileInfo> LocalizationFiles { get; }
    public ObservableCollection<ITranslationSession> TranslationSessions { get; } = [];
    private ITranslationSession? _CurrentTranslationSession;
    public ITranslationSession? CurrentTranslationSession {
        get => this._CurrentTranslationSession;
        set => this.SetProperty(ref this._CurrentTranslationSession, value, this.CurrentTranslationSessionChanged);
    }
    public string InstallPath { get; }
    public bool HasTranslationSessions => this.TranslationSessions.Any();
    private bool _IsAppUseAble;
    public bool IsAppUseAble {
        get => this._IsAppUseAble;
        private set => this.SetProperty(ref this._IsAppUseAble, value);
    }


    private string? _DatabaseError;
    public string? DatabaseError {
        get => this._DatabaseError;
        set => this.SetProperty(ref this._DatabaseError, value, this.OnDatabaseErrorChange);
    }

    public bool HasDatabaseError => !this.HasNoDatabaseError;
    public bool HasNoDatabaseError => StringHelper.IsNullOrWhiteSpaceOrEmpty(this.DatabaseError);


    public TranslationSessionManager(IRegionManager regionManager,
                                     IViewConfigurations viewConfigurations,
                                     InstallPathDetector installPathDetector,
                                     ILocalizationFileService localizationFilesService,
                                     ITranslationsDatabaseService db) {
        this.regionManager = regionManager;
        this.viewConfigurations = viewConfigurations;
        this.InstallPathDetector = installPathDetector;
        this.onError = (error) => this.DatabaseError = error;
        try {
            this.InstallPath = this.InstallPathDetector.DetectInstallPath();
            this.IsAppUseAble = true;
        } catch {
            this.IsAppUseAble = false;
            return;
        }
        this.LocalizationFilesService = localizationFilesService;
        this.db = db;
        this.LocalizationFiles = this.LocalizationFilesService.GetLocalizationFiles();
        this.BaseLocalizationFile = this.GetLocalizationFile(AppConfigurationManager.LeadingLocFileName);
        this.db.EnrichTranslationSessions(this, this.onError);
        if (this.HasDatabaseError) {
            // see StartView-xaml-code
            // see OnDatabaseErrorChange
        }
    }

    private LocalizationFile GetLocalizationFile(string fileName) {
        FileInfo baseLocalizationFileInfo = this.LocalizationFiles.Where(item => item.Name == fileName).First();
        return this.LocalizationFilesService.GetLocalizationFile(baseLocalizationFileInfo);
    }

    public void AddTranslationSession(ITranslationSession translationSession) {
        this.TranslationSessions.Add(translationSession);
        this.CurrentTranslationSession = translationSession;
        this.RaisePropertyChanged(nameof(this.HasTranslationSessions));
    }
    public void CurrentTranslationSessionChanged() {
        // always raise!!!
        this.RaisePropertyChanged(nameof(this.HasTranslationSessions));
        foreach (ITranslationSession translationSession in this.TranslationSessions) {
            translationSession.Localizations.Clear();
        }
        if (this.CurrentTranslationSession == null) {
            return;
        }
        foreach (ILocalizationEntry item in this.BaseLocalizationFile.Localizations) {
            // copy item, otherwise changes are reflected into BaseLocalizationFile.Localizations
            this.CurrentTranslationSession.Localizations.Add(new LocalizationEntry(item));
        }
        this.db.EnrichSavedTranslations(this.CurrentTranslationSession, this.onError);
        if (this.HasDatabaseError) {
            // see SessionManagementView-xaml-code
            // see OnDatabaseErrorChange
            return;
        }
        FileInfo mergeFileInfo = this.LocalizationFiles.Where(item => item.Name == this.CurrentTranslationSession.MergeLocalizationFileName).First();
        LocalizationFile mergeFile = this.LocalizationFilesService.GetLocalizationFile(mergeFileInfo);
        foreach (ILocalizationEntry mergeEntry in mergeFile.Localizations) {
            ILocalizationEntry item = this.CurrentTranslationSession.Localizations.Where(item => item.Key == mergeEntry.Key).First();
            item.ValueMerge = mergeEntry.Value;
            item.ValueMergeLanguageCode = this.CurrentTranslationSession.MergeLanguageCode;
        }
    }

    public ILocalizationFile GetForExport(bool json) {
        ILocalizationFile mergeLocalizationFile = this.GetLocalizationFile(this.CurrentTranslationSession.MergeLocalizationFileName);
        ILocalizationFile merged = new LocalizationFile(this.CurrentTranslationSession.OverwriteLocalizationFileName,
                                                        mergeLocalizationFile.FileHeader,
                                                        this.CurrentTranslationSession.OverwriteLocalizationNameEN,
                                                        this.CurrentTranslationSession.OverwriteLocalizationLocaleID,
                                                        this.CurrentTranslationSession.OverwriteLocalizationNameLocalized);
        merged.Indices.Clear();
        merged.Localizations.Clear();
        DictionaryHelper.AddAll(mergeLocalizationFile.Indices, merged.Indices);
        List<ILocalizationEntry>? localizations = null;
        if (json) {
            localizations =
                this.CurrentTranslationSession.Localizations
                    .Where(item => !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Key) && (!StringHelper.IsNullOrWhiteSpaceOrEmpty(item.Translation) || !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.ValueMerge))).ToList();
        } else {
            localizations =
                this.CurrentTranslationSession.Localizations
                    .Where(item => !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.ValueMerge)).ToList();
        }
        merged.Localizations.AddRange(localizations);
        // addMergeValues has to be true!!!
        IDictionary<string, string> dictionary = merged.GetLocalizationsAsDictionary(true);
        IndexCountHelper.FillIndexCountsAndAutocorrect(dictionary,
                                                       merged.Indices);
        return merged;
    }

    public void Insert(ITranslationSession? session) {
        if (session == null) {
            return;
        }
        this.db.UpsertTranslationSession(session, this.onError);
        if (!this.HasDatabaseError) {
            this.AddTranslationSession(session);
        }
    }

    public void Update(ITranslationSession? session) {
        if (session == null) {
            return;
        }
        this.db.UpsertTranslationSession(session, this.onError);
        if (!this.HasDatabaseError) {
            this.CurrentTranslationSessionChanged();
        }
    }

    public void Delete(ITranslationSession? session) {
        if (session == null) {
            return;
        }
        this.db.BackUpIfExists(DatabaseBackUpIndicators.BEFORE_DELETE_SESSION);
        this.db.DeleteTranslationSession(session, this.onError);
        if (!this.HasDatabaseError) {
            this.TranslationSessions.Remove(session);
            if (this.TranslationSessions.Any()) {
                this.CurrentTranslationSession = this.TranslationSessions.Last();
            } else {
                this.CurrentTranslationSession = null;
            }
            this.CurrentTranslationSessionChanged();
        }
    }

    public void SaveCurrentTranslationSessionsTranslations() {
        this.db.SaveTranslations(this.CurrentTranslationSession, this.onError);
    }

    private void OnDatabaseErrorChange() {
        this.IsAppUseAble = false;
        this.RaisePropertyChanged(nameof(this.HasDatabaseError));
        this.RaisePropertyChanged(nameof(this.HasNoDatabaseError));
        IViewConfiguration? viewConfiguration = this.viewConfigurations.GetStartViewConfiguration();
        if (viewConfiguration != null) {
            this.regionManager.RequestNavigate(AppConfigurationManager.AppMainRegion, viewConfiguration.Name);
        }
    }

    public ITranslationSession GetNewTranslationSession() {
        return new TranslationSession();
    }

    public bool ExistsKeyInCurrentTranslationSession(string key) {
        return this.CurrentTranslationSession.Localizations.Where(item => item.Key == key).Any();
    }

    public IndexCountHelperValidationResult IsIndexKeyValid(string key, string? keyOrigin) {
        ObservableCollection<ILocalizationEntry> localizationDictionary = this.CurrentTranslationSession.Localizations;
        return IndexCountHelper.ValidateForKey(localizationDictionary, key);
    }

    public ITranslationSession? CloneCurrent(bool includeDictionary) {
        return new TranslationSession(this.CurrentTranslationSession, includeDictionary);
    }

    public void UpdateCurrentWith(ITranslationSession? session) {
        this.CurrentTranslationSession.UpdateWith(session);
        this.Update(this.CurrentTranslationSession);
    }
}
