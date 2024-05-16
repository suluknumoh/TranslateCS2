using System;
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
            translationSession.Localizations.Clear();
        }
        if (this.CurrentTranslationSession == null) {
            return;
        }
        foreach (ILocalizationEntry item in this.BaseLocalizationFile.Localizations) {
            // copy item, otherwise changes are reflected into BaseLocalizationFile.Localizations
            this.CurrentTranslationSession.Localizations.Add(new LocalizationEntry(item));
        }
        this._db.EnrichSavedTranslations(this.CurrentTranslationSession, this._onError);
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

    public ILocalizationFile GetForExport(bool json, IList<KeyValuePair<string, int>> errors) {
        ILocalizationFile mergeLocalizationFile = this.GetLocalizationFile(this.CurrentTranslationSession.MergeLocalizationFileName);
        ILocalizationFile merged = new LocalizationFile(this.CurrentTranslationSession.OverwriteLocalizationFileName,
                                                        mergeLocalizationFile.FileHeader,
                                                        this.CurrentTranslationSession.OverwriteLocalizationNameEN,
                                                        this.CurrentTranslationSession.OverwriteLocalizationLocaleID,
                                                        this.CurrentTranslationSession.OverwriteLocalizationNameLocalized);
        merged.Indizes.Clear();
        merged.Localizations.Clear();
        DictionaryHelper.AddAll(mergeLocalizationFile.Indizes, merged.Indizes);
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
        IndexCountHelper.FillIndexCountsFromLocalizationDictionary(dictionary,
                                                                   merged.Indizes,
                                                                   errors);
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
        return this.CurrentTranslationSession.Localizations.Where(item => item.Key == key).Any();
    }

    public (bool, KeyValuePair<string, int>?) IsIndexKeyValid(string key, string? keyOrigin) {
        List<KeyValuePair<string, int>> errors = [];
        IDictionary<string, string> localizationDictionary = this.CurrentTranslationSession.GetLocalizationsAsDictionary(true);
        if (keyOrigin != null) {
            // keyOrigin is null on add entry
            // keyOrigin is NOT null on edit entry and it has to be removed to validate the correct order
            localizationDictionary.Remove(keyOrigin);
        }
        // value is irrelevant
        // the key needs to be checked
        localizationDictionary.Add(key, String.Empty);
        Dictionary<string, int> indexCounts = [];
        IndexCountHelper.FillIndexCountsFromLocalizationDictionary(localizationDictionary,
                                                                   indexCounts,
                                                                   errors);
        return (errors.Count == 0, errors.Count == 0 ? null : errors[0]);
    }

    public ITranslationSession? CloneCurrent(bool includeDictionary) {
        return new TranslationSession(this.CurrentTranslationSession, includeDictionary);
    }

    public void UpdateCurrentWith(ITranslationSession? session) {
        this.CurrentTranslationSession.UpdateWith(session);
        this.Update(this.CurrentTranslationSession);
    }
}
