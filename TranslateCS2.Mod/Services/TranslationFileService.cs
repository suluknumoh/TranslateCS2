using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;

using Colossal.IO.AssetDatabase;
using Colossal.Localization;

using Game.SceneFlow;

using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Loggers;
using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod.Services;
internal class TranslationFileService {
    private static readonly LocalizationManager LocalizationManager = GameManager.instance.localizationManager;
    private string SeeWhatItIs => "just to see, what it is:";
    private string FailedToLoad => "failed to load:";
    private string FailedToUnLoad => "failed to unload:";
    private string JsonExtension => ".json";
    private string SearchPattern => $"*{this.JsonExtension}";
    private IList<TranslationFile> TranslationFiles { get; } = [];
    private ExecutableAsset Asset { get; }
    private bool IsOverwrite => this.Settings?.IsOverwrite ?? false;
    public string AssetDirectoryPath { get; }
    public ModSettings? Settings { get; set; }
    public TranslationFileService(ExecutableAsset asset) {
        this.Asset = asset;
        this.AssetDirectoryPath = this.Asset.path.Replace($"{Mod.Name}{ExecutableAsset.kExtension}", String.Empty);
    }
    private IList<TranslationFile> GetFiles() {
        List<TranslationFile> files = [];
        IEnumerable<string> translationFilePaths = Directory.EnumerateFiles(this.AssetDirectoryPath, this.SearchPattern);
        foreach (string translationFilePath in translationFilePaths) {
            try {
                string name = translationFilePath
                    .Replace(this.AssetDirectoryPath, String.Empty)
                    .Replace(this.JsonExtension, String.Empty);
                if (!this.IsLoadAble(name)) {
                    continue;
                }
                TranslationFile translationFile = new TranslationFile(name, translationFilePath);
                files.Add(translationFile);
            } catch (Exception ex) {
                Mod.Logger.LogError(typeof(TranslationFileService),
                                    this.FailedToLoad,
                                    [translationFilePath, ex]);
            }
        }
        return files;
    }

    private bool IsLoadAble(string id) {
        if (id.Contains("-")) {
            return CultureInfo.GetCultures(CultureTypes.AllCultures)
                .Where(item => item.Name.Equals(id, StringComparison.OrdinalIgnoreCase))
                .Any()
                && !this.TranslationFiles
                .Where(item => item.Name.Equals(id, StringComparison.OrdinalIgnoreCase))
                .Any();
        }
        return false;
    }

    private bool TryToLoadTranslationFile(TranslationFile translationFile, bool isReload) {
        try {
            if (isReload) {
                translationFile.ReInit();
            }
            if (!translationFile.IsOK) {
                Mod.Logger.LogError(typeof(TranslationFileService),
                                    this.FailedToLoad,
                                    [translationFile]);
                return false;
            }
            if (!isReload && !translationFile.MapsToExisting) {
                this.TryToAddLocale(translationFile);
            }
            this.TryToAddSource(translationFile);
            return true;
        } catch (Exception ex) {
            Mod.Logger.LogError(typeof(TranslationFileService),
                                this.FailedToLoad,
                                [translationFile, ex]);
            return false;
        }
    }

    private void TryToAddSource(TranslationFile translationFile) {
        try {
            if (translationFile.MapsToExisting && !this.IsOverwrite) {
                return;
            }
            LocalizationManager.AddSource(translationFile.LocaleId,
                                          translationFile);
        } catch {
            Mod.Logger.LogError(typeof(TranslationFileService),
                                this.FailedToLoad,
                                [translationFile]);
            this.TryToUnload(translationFile, false);
            throw;
        }
    }

    private void TryToAddLocale(TranslationFile translationFile) {
        try {
            if (LocaleHelper.MapsToExisting(translationFile.LocaleId)
                || SystemLanguageHelper.IsSystemLanguageInUse(translationFile.LanguageCulture.Language)) {
                return;
            }
            LocalizationManager.AddLocale(translationFile.LocaleId,
                                          translationFile.LanguageCulture.Language,
                                          translationFile.LocaleName);
        } catch {
            Mod.Logger.LogError(typeof(TranslationFileService),
                                this.FailedToLoad,
                                [translationFile]);
            LocalizationManager.RemoveLocale(translationFile.LocaleId);
            throw;
        }
    }

    public void Load() {
        IList<TranslationFile> translationFiles = this.GetFiles();
        foreach (TranslationFile translationFile in translationFiles) {
            bool loaded = this.TryToLoadTranslationFile(translationFile, false);
            if (loaded) {
                this.TranslationFiles.Add(translationFile);
            }
        }
    }

    public void Reload() {
        foreach (TranslationFile translationFile in this.TranslationFiles) {
            this.TryToUnload(translationFile, true);
            this.TryToLoadTranslationFile(translationFile, true);
        }
    }

    private void TryToUnload(TranslationFile translationFile, bool isReload) {
        try {
            LocalizationManager.RemoveSource(translationFile.LocaleId,
                                             translationFile);
            if (!isReload && !translationFile.MapsToExisting) {
                LocalizationManager.RemoveLocale(translationFile.LocaleId);
            }
        } catch (Exception ex) {
            Mod.Logger.LogError(typeof(TranslationFileService),
                                this.FailedToUnLoad,
                                [ex, translationFile]);
        }
    }

    private void TryToClear(TranslationFile translationFile) {
        try {
            if (!translationFile.MapsToExisting) {
                return;
            }
            LocalizationManager.RemoveSource(translationFile.LocaleId,
                                             translationFile);
        } catch (Exception ex) {
            Mod.Logger.LogError(typeof(TranslationFileService),
                                this.FailedToUnLoad,
                                [ex, translationFile]);
        }
    }

    public void ClearOverwritten() {
        foreach (TranslationFile translationFile in this.TranslationFiles) {
            this.TryToClear(translationFile);
        }
    }
}
