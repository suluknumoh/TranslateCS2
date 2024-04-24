using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using Prism.Mvvm;
using Prism.Regions;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Configurations.Views;
using TranslateCS2.Core.Helpers;
using TranslateCS2.Core.Services.Databases;
using TranslateCS2.Core.Services.InstallPaths;
using TranslateCS2.Core.Services.LocalizationFiles;

namespace TranslateCS2.Core.Sessions;
internal class TranslationSessionManager : BindableBase, ITranslationSessionManager {
    private readonly IRegionManager _regionManager;
    private readonly IViewConfigurations _viewConfigurations;
    private readonly ITranslationsDatabaseService.OnErrorCallBack _onError;
    private readonly ITranslationsDatabaseService _db;
    public ILocalizationFile BaseLocalizationFile { get; }
    public InstallPathDetector InstallPathDetector { get; }
    public LocalizationFilesService LocalizationFilesService { get; }
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
                                     LocalizationFilesService localizationFilesService,
                                     ITranslationsDatabaseService db) {
        this._regionManager = regionManager;
        this._viewConfigurations = viewConfigurations;
        this.InstallPathDetector = installPathDetector;
        this._onError = (error) => this.DatabaseError = error;
        try {
            this.InstallPath = this.InstallPathDetector.DetectInstallPath();
            this.IsAppUseAble = true;
        } catch {
            this.IsAppUseAble = false;
            return;
        }
        this.LocalizationFilesService = localizationFilesService;
        this._db = db;
        this.LocalizationFiles = this.LocalizationFilesService.GetLocalizationFiles();
        this.BaseLocalizationFile = this.GetLocalizationFile(AppConfigurationManager.LeadingLocFileName);
        this._db.EnrichTranslationSessions(this, this._onError);
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
            translationSession.LocalizationDictionary.Clear();
        }
        if (this.CurrentTranslationSession == null) {
            return;
        }
        foreach (ILocalizationDictionaryEntry item in this.BaseLocalizationFile.LocalizationDictionary) {
            // copy item, otherwise changes are reflected into BaseLocalizationFile.LocalizationDictionary
            this.CurrentTranslationSession.LocalizationDictionary.Add(new LocalizationDictionaryEntry(item));
        }
        this._db.EnrichSavedTranslations(this.CurrentTranslationSession, this._onError);
        if (this.HasDatabaseError) {
            // see SessionManagementView-xaml-code
            // see OnDatabaseErrorChange
            return;
        }
        FileInfo mergeFileInfo = this.LocalizationFiles.Where(item => item.Name == this.CurrentTranslationSession.MergeLocalizationFileName).First();
        LocalizationFile mergeFile = this.LocalizationFilesService.GetLocalizationFile(mergeFileInfo);
        foreach (ILocalizationDictionaryEntry mergeEntry in mergeFile.LocalizationDictionary) {
            ILocalizationDictionaryEntry item = this.CurrentTranslationSession.LocalizationDictionary.Where(item => item.Key == mergeEntry.Key).First();
            item.ValueMerge = mergeEntry.Value;
            item.ValueMergeLanguageCode = this.CurrentTranslationSession.MergeLanguageCode;
        }
    }

    public ILocalizationFile GetForExport() {
        ILocalizationFile mergeLocalizationFile = this.GetLocalizationFile(this.CurrentTranslationSession.MergeLocalizationFileName);
        ILocalizationFile merged = new LocalizationFile(this.CurrentTranslationSession.OverwriteLocalizationFileName,
                                                        mergeLocalizationFile.FileHeader,
                                                        this.CurrentTranslationSession.OverwriteLocalizationNameEN,
                                                        this.CurrentTranslationSession.OverwriteLocalizationLocaleID,
                                                        this.CurrentTranslationSession.OverwriteLocalizationNameLocalized);
        merged.Indizes.Clear();
        merged.Indizes.AddRange(mergeLocalizationFile.Indizes);
        merged.LocalizationDictionary.Clear();
        List<ILocalizationDictionaryEntry> dic = this.CurrentTranslationSession.LocalizationDictionary.Where(item => !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.ValueMerge)).ToList();
        merged.LocalizationDictionary.AddRange(dic);
        return merged;
    }

    public void Insert(ITranslationSession? session) {
        if (session == null) {
            return;
        }
        this._db.UpsertTranslationSession(session, this._onError);
        if (!this.HasDatabaseError) {
            this.AddTranslationSession(session);
        }
    }

    public void Update(ITranslationSession? session) {
        if (session == null) {
            return;
        }
        this._db.UpsertTranslationSession(session, this._onError);
        if (!this.HasDatabaseError) {
            this.CurrentTranslationSessionChanged();
        }
    }

    public void Delete(ITranslationSession? session) {
        if (session == null) {
            return;
        }
        this._db.BackUpIfExists(DatabaseBackUpIndicators.BEFORE_DELETE_SESSION);
        this._db.DeleteTranslationSession(session, this._onError);
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
        this._db.SaveTranslations(this.CurrentTranslationSession, this._onError);
    }

    private void OnDatabaseErrorChange() {
        this.IsAppUseAble = false;
        this.RaisePropertyChanged(nameof(this.HasDatabaseError));
        this.RaisePropertyChanged(nameof(this.HasNoDatabaseError));
        IViewConfiguration? viewConfiguration = this._viewConfigurations.GetStartViewConfiguration();
        if (viewConfiguration != null) {
            this._regionManager.RequestNavigate(AppConfigurationManager.AppMainRegion, viewConfiguration.Name);
        }
    }

    public ITranslationSession GetNewTranslationSession() {
        return new TranslationSession();
    }

    public bool ExistsKeyInCurrentTranslationSession(string key) {
        return this.CurrentTranslationSession.LocalizationDictionary.Where(item => item.Key == key).Any();
    }
}
