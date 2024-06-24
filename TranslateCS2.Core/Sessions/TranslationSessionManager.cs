using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using Prism.Mvvm;
using Prism.Regions;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Configurations.Views;
using TranslateCS2.Core.Models.Localizations;
using TranslateCS2.Core.Services.Databases;
using TranslateCS2.Core.Services.InstallPaths;
using TranslateCS2.Inf;
using TranslateCS2.Inf.Models;
using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Inf.Services.Localizations;

namespace TranslateCS2.Core.Sessions;
internal class TranslationSessionManager : BindableBase, ITranslationSessionManager {
    private readonly IRegionManager regionManager;
    private readonly IViewConfigurations viewConfigurations;
    private readonly ITranslationsDatabaseService.OnErrorCallBack onError;
    private readonly ITranslationsDatabaseService db;
    private readonly IInstallPathDetector InstallPathDetector;
    public MyLocalization<IAppLocFileEntry> BaseLocalizationFile { get; }
    public LocFileService<IAppLocFileEntry> LocalizationFilesService { get; }
    public IEnumerable<FileInfo> LocalizationFiles { get; }
    public ObservableCollection<ITranslationSession> TranslationSessions { get; } = [];
    private ITranslationSession? _CurrentTranslationSession;
    public ITranslationSession? CurrentTranslationSession {
        get => this._CurrentTranslationSession;
        set => this.SetProperty(ref this._CurrentTranslationSession, value, this.CurrentTranslationSessionChanged);
    }
    public string InstallPath => this.InstallPathDetector.InstallPath;
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
                                     IInstallPathDetector installPathDetector,
                                     LocFileService<IAppLocFileEntry> localizationFilesService,
                                     ITranslationsDatabaseService db) {
        this.regionManager = regionManager;
        this.viewConfigurations = viewConfigurations;
        this.InstallPathDetector = installPathDetector;
        this.LocalizationFilesService = localizationFilesService;
        this.db = db;
        this.onError = (error) => this.DatabaseError = error;
        this.IsAppUseAble = this.InstallPathDetector.Detect();
        if (!this.IsAppUseAble) {
            return;
        }
        this.LocalizationFiles = this.LocalizationFilesService.GetLocalizationFiles();
        this.BaseLocalizationFile = this.GetLocalizationFile(AppConfigurationManager.LeadingLocFileName);
        this.db.EnrichTranslationSessions(this, this.onError);
        if (this.HasDatabaseError) {
            // see StartView-xaml-code
            // see OnDatabaseErrorChange
        }
    }

    private MyLocalization<IAppLocFileEntry> GetLocalizationFile(string fileName) {
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
        if (this.CurrentTranslationSession is null) {
            return;
        }
        foreach (KeyValuePair<string, IAppLocFileEntry> item in this.BaseLocalizationFile.Source.Localizations) {
            // copy item, otherwise changes are reflected into BaseLocalizationFile.Localizations
            IAppLocFileEntry clone = item.Value.Clone();
            this.CurrentTranslationSession.Localizations.Add(new KeyValuePair<string, IAppLocFileEntry>(clone.Key.Key, clone));
        }
        this.db.EnrichSavedTranslations(this.CurrentTranslationSession, this.onError);
        if (this.HasDatabaseError) {
            // see SessionManagementView-xaml-code
            // see OnDatabaseErrorChange
            return;
        }
        FileInfo mergeFileInfo = this.LocalizationFiles.Where(item => item.Name == this.CurrentTranslationSession.MergeLocalizationFileName).First();
        MyLocalization<IAppLocFileEntry> mergeFile = this.LocalizationFilesService.GetLocalizationFile(mergeFileInfo);
        foreach (KeyValuePair<string, IAppLocFileEntry> mergeEntry in mergeFile.Source.Localizations) {
            IEnumerable<KeyValuePair<string, IAppLocFileEntry>> entries = this.CurrentTranslationSession.Localizations.Where(item => item.Key.Equals(mergeEntry.Key));
            if (entries.Any()) {
                IAppLocFileEntry item = entries.First().Value;
                item.ValueMerge = mergeEntry.Value.Value;
            }
        }
    }

    public void Insert(ITranslationSession? session) {
        if (session is null) {
            return;
        }
        this.db.UpsertTranslationSession(session, this.onError);
        if (!this.HasDatabaseError) {
            this.AddTranslationSession(session);
        }
    }

    public void Update(ITranslationSession? session) {
        if (session is null) {
            return;
        }
        this.db.UpsertTranslationSession(session, this.onError);
        if (!this.HasDatabaseError) {
            this.CurrentTranslationSessionChanged();
        }
    }

    public void Delete(ITranslationSession? session) {
        if (session is null) {
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
        if (viewConfiguration is not null) {
            this.regionManager.RequestNavigate(AppConfigurationManager.AppMainRegion, viewConfiguration.Name);
        }
    }

    public ITranslationSession GetNewTranslationSession() {
        return new TranslationSession();
    }

    public bool ExistsKeyInCurrentTranslationSession(string key) {
        return this.CurrentTranslationSession.Localizations.Where(item => item.Value.Key.Key == key).Any();
    }

    public IndexCountHelperValidationResult IsIndexKeyValid(string key, string? keyOrigin) {
        ICollection<KeyValuePair<string, IAppLocFileEntry>> localizationDictionary = this.CurrentTranslationSession.Localizations;
        return IndexCountHelper.ValidateForKey(localizationDictionary, key);
    }

    public ITranslationSession? CloneCurrent() {
        ITranslationSession clone = new TranslationSession();
        clone.UpdateWith(this.CurrentTranslationSession);
        return clone;
    }

    public void UpdateCurrentWith(ITranslationSession? session) {
        this.CurrentTranslationSession.UpdateWith(session);
        this.Update(this.CurrentTranslationSession);
    }
}
