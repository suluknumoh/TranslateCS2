using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;

using Prism.Mvvm;

using TranslateCS2.Configurations;
using TranslateCS2.Databases;
using TranslateCS2.Models.LocDictionary;
using TranslateCS2.Services;

namespace TranslateCS2.Models.Sessions;
internal class TranslationSessionManager : BindableBase {
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
    public bool IsAppUseAble { get; }
    public TranslationSessionManager(InstallPathDetector installPathDetector, LocalizationFilesService localizationFilesService) {
        try {
            this.InstallPath = installPathDetector.DetectInstallPath();
            this.IsAppUseAble = true;
        } catch {
            this.IsAppUseAble = false;
            return;
        }
        this.InstallPathDetector = installPathDetector;
        this.LocalizationFilesService = localizationFilesService;
        this.LocalizationFiles = this.LocalizationFilesService.GetLocalizationFiles();
        this.BaseLocalizationFile = this.GetLocalizationFile(AppConfigurationManager.LeadingLocFileName);
        TranslationsDB.EnrichTranslationSessions(this);
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
    private void CurrentTranslationSessionChanged() {
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
        TranslationsDB.EnrichSavedTranslations(this.CurrentTranslationSession);
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
        List<LocalizationDictionaryEntry> dic = this.CurrentTranslationSession.LocalizationDictionary.Where(item => !String.IsNullOrEmpty(item.ValueMerge) && !String.IsNullOrWhiteSpace(item.ValueMerge)).ToList();
        merged.LocalizationDictionary.AddRange(dic);
        return merged;
    }

    public void Insert(TranslationSession? session) {
        if (session == null) {
            return;
        }
        TranslationsDB.AddTranslationSession(session);
        this.AddTranslationSession(session);
    }

    public void Update(TranslationSession? session) {
        if (session == null) {
            return;
        }
        TranslationsDB.UpdateTranslationSession(session);
        this.CurrentTranslationSessionChanged();
    }

    public void SaveCurrentTranslationSessionsTranslations() {
        TranslationsDB.SaveTranslations(this.CurrentTranslationSession);
    }
}
