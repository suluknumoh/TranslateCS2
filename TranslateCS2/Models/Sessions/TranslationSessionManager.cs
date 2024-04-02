using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using Prism.Mvvm;
using Prism.Regions;

using TranslateCS2.Configurations;
using TranslateCS2.Configurations.Views;
using TranslateCS2.Databases;
using TranslateCS2.Helpers;
using TranslateCS2.Models.Imports;
using TranslateCS2.Models.LocDictionary;
using TranslateCS2.Services;
using TranslateCS2.ViewModels.Works;

namespace TranslateCS2.Models.Sessions;
internal class TranslationSessionManager : BindableBase {
    private readonly IRegionManager _regionManager;
    private readonly ViewConfigurations _viewConfigurations;
    private readonly TranslationsDB.OnErrorCallBack _onError;
    public LocalizationFile BaseLocalizationFile { get; }
    public InstallPathDetector InstallPathDetector { get; }
    public LocalizationFilesService LocalizationFilesService { get; }
    public IEnumerable<FileInfo> LocalizationFiles { get; }
    public ObservableCollection<TranslationSession> TranslationSessions { get; } = [];
    private TranslationSession? _CurrentTranslationSession;
    public TranslationSession? CurrentTranslationSession {
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
                                     ViewConfigurations viewConfigurations,
                                     InstallPathDetector installPathDetector,
                                     LocalizationFilesService localizationFilesService) {
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
        this.LocalizationFiles = this.LocalizationFilesService.GetLocalizationFiles();
        this.BaseLocalizationFile = this.GetLocalizationFile(AppConfigurationManager.LeadingLocFileName);
        TranslationsDB.EnrichTranslationSessions(this, this._onError);
        if (this.HasDatabaseError) {
            // see StartView-xaml-code
            // see OnDatabaseErrorChange
        }
    }

    private LocalizationFile GetLocalizationFile(string fileName) {
        FileInfo baseLocalizationFileInfo = this.LocalizationFiles.Where(item => item.Name == fileName).First();
        return this.LocalizationFilesService.GetLocalizationFile(baseLocalizationFileInfo);
    }

    public void AddTranslationSession(TranslationSession translationSession) {
        this.TranslationSessions.Add(translationSession);
        this.CurrentTranslationSession = translationSession;
        this.RaisePropertyChanged(nameof(this.HasTranslationSessions));
    }
    public void CurrentTranslationSessionChanged() {
        // always raise!!!
        this.RaisePropertyChanged(nameof(this.HasTranslationSessions));
        foreach (TranslationSession translationSession in this.TranslationSessions) {
            translationSession.LocalizationDictionary.Clear();
        }
        if (this.CurrentTranslationSession == null) {
            return;
        }
        foreach (LocalizationDictionaryEntry item in this.BaseLocalizationFile.LocalizationDictionary) {
            // copy item, otherwise changes are reflected into BaseLocalizationFile.LocalizationDictionary
            this.CurrentTranslationSession.LocalizationDictionary.Add(new LocalizationDictionaryEntry(item));
        }
        TranslationsDB.EnrichSavedTranslations(this.CurrentTranslationSession, this._onError);
        if (this.HasDatabaseError) {
            // see SessionManagementView-xaml-code
            // see OnDatabaseErrorChange
            return;
        }
        FileInfo mergeFileInfo = this.LocalizationFiles.Where(item => item.Name == this.CurrentTranslationSession.MergeLocalizationFileName).First();
        LocalizationFile mergeFile = this.LocalizationFilesService.GetLocalizationFile(mergeFileInfo);
        foreach (LocalizationDictionaryEntry mergeEntry in mergeFile.LocalizationDictionary) {
            LocalizationDictionaryEntry item = this.CurrentTranslationSession.LocalizationDictionary.Where(item => item.Key == mergeEntry.Key).First();
            item.ValueMerge = mergeEntry.Value;
        }
    }

    public LocalizationFile GetForExport() {
        LocalizationFile mergeLocalizationFile = this.GetLocalizationFile(this.CurrentTranslationSession.MergeLocalizationFileName);
        LocalizationFile merged = new LocalizationFile(this.CurrentTranslationSession.OverwriteLocalizationFileName,
                                                       mergeLocalizationFile.FileHeader,
                                                       this.CurrentTranslationSession.OverwriteLocalizationNameEN,
                                                       this.CurrentTranslationSession.OverwriteLocalizationLocaleID,
                                                       this.CurrentTranslationSession.OverwriteLocalizationNameLocalized);
        merged.Indizes.Clear();
        merged.Indizes.AddRange(mergeLocalizationFile.Indizes);
        merged.LocalizationDictionary.Clear();
        List<LocalizationDictionaryEntry> dic = this.CurrentTranslationSession.LocalizationDictionary.Where(item => !StringHelper.IsNullOrWhiteSpaceOrEmpty(item.ValueMerge)).ToList();
        merged.LocalizationDictionary.AddRange(dic);
        return merged;
    }

    public void Insert(TranslationSession? session) {
        if (session == null) {
            return;
        }
        TranslationsDB.UpsertTranslationSession(session, this._onError);
        if (!this.HasDatabaseError) {
            this.AddTranslationSession(session);
        }
    }

    public void Update(TranslationSession? session) {
        if (session == null) {
            return;
        }
        TranslationsDB.UpsertTranslationSession(session, this._onError);
        if (!this.HasDatabaseError) {
            this.CurrentTranslationSessionChanged();
        }
    }

    public void Delete(TranslationSession? session) {
        if (session == null) {
            return;
        }
        DatabaseHelper.BackUpIfExists(DatabaseBackUpIndicators.BEFORE_DELETE_SESSION);
        TranslationsDB.DeleteTranslationSession(session, this._onError);
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
        TranslationsDB.SaveTranslations(this.CurrentTranslationSession, this._onError);
    }

    private void OnDatabaseErrorChange() {
        this.IsAppUseAble = false;
        this.RaisePropertyChanged(nameof(this.HasDatabaseError));
        this.RaisePropertyChanged(nameof(this.HasNoDatabaseError));
        IViewConfiguration? viewConfiguration = this._viewConfigurations.GetViewConfiguration<StartViewModel>();
        this._regionManager.RequestNavigate(AppConfigurationManager.AppMainRegion, viewConfiguration.Name);
    }

    internal void HandleImported(IList<CompareExistingImportedTranslation> preview, ImportModes importMode) {
        foreach (LocalizationDictionaryEntry currentEntry in this.CurrentTranslationSession.LocalizationDictionary) {
            foreach (CompareExistingImportedTranslation importedItem in preview) {
                if (importedItem.Key == currentEntry.Key) {
                    switch (importMode) {
                        case ImportModes.NEW:
                            // set all imported
                            currentEntry.Translation = importedItem.TranslationImported;
                            break;
                        case ImportModes.LeftJoin:
                            // set missing imported; all existing + imported that are missing
                            // preview leads!
                            // preview knows about Existing and Imported!
                            // take care of method name 'is ... EXISTING available'
                            if (!importedItem.IsTranslationExistingAvailable) {
                                // only set if no translation existed
                                currentEntry.Translation = importedItem.TranslationImported;
                            } else {
                                // just for clarififaction
                                // keep current!
                            }
                            break;
                        case ImportModes.RightJoin:
                            // set all imported; all imported + existing that werent imported
                            // preview leads!
                            // preview knows about Existing and Imported!
                            // take care of method name 'is ... IMPORTED available'
                            if (importedItem.IsTranslationImportedAvailable) {
                                // only set, if a translation is imported
                                currentEntry.Translation = importedItem.TranslationImported;
                            } else {
                                // just for clarififaction
                                // keep current!
                            }
                            break;
                    }
                    continue;
                }
            }
        }
    }
}
