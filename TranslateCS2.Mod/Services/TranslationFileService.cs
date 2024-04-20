using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Colossal.IO.AssetDatabase;
using Colossal.Localization;

using Game.SceneFlow;

using TranslateCS2.Mod.Loggers;
using TranslateCS2.Mod.Models;

using UnityEngine;

namespace TranslateCS2.Mod.Services;
internal class TranslationFileService {
    private static readonly LocalizationManager LocalizationManager = GameManager.instance.localizationManager;
    public static IList<string> BuiltIn { get; } = [
        "de-DE",
        "en-US",
        "es-ES",
        "fr-FR",
        "it-IT",
        "ja-JP",
        "ko-KR",
        "pl-PL",
        "pt-BR",
        "ru-RU",
        "zh-HANS",
        "zh_HANT"
    ];
    private string SeeWhatItIs => "just to see, what it is:\r\n{0}";
    private string FailedToLoad => "failed to load: {0}\r\n{1}";
    private string FailedToUnLoad => "failed to unload:\r\n{0}\r\n{1}";
    private string JsonExtension => ".json";
    private string SearchPattern => $"*{this.JsonExtension}";
    private IList<TranslationFile> ModLocales { get; } = [];
    private ExecutableAsset Asset { get; }
    public string AssetDirectoryPath { get; }
    public TranslationFileService(ExecutableAsset asset) {
        this.Asset = asset;
        this.AssetDirectoryPath = this.Asset.path.Replace($"{Mod.Name}{ExecutableAsset.kExtension}", String.Empty);
    }
    private IList<TranslationFile> GetFiles() {
        List<TranslationFile> files = [];
        IEnumerable<string> translationFilePaths = Directory.EnumerateFiles(this.AssetDirectoryPath, this.SearchPattern);
        foreach (string translationFilePath in translationFilePaths) {
            string name = translationFilePath
                .Replace(this.AssetDirectoryPath, String.Empty)
                .Replace(this.JsonExtension, String.Empty);
            TranslationFile translationFile = new TranslationFile(name, translationFilePath);
            files.Add(translationFile);
        }
        return files;
    }

    private IList<TranslationFile> OnlyBuiltIn(IList<TranslationFile> translationFiles) {
        return translationFiles.Where(translationFile => BuiltIn.Contains(translationFile.Name)).ToList();
    }

    private IList<TranslationFile> WithoutBuiltIn(IList<TranslationFile> translationFiles) {
        return translationFiles.Where(translationFile => !BuiltIn.Contains(translationFile.Name)).ToList();
    }
    private void LoadFiles(IList<TranslationFile> translationFiles, bool isReload) {
        foreach (TranslationFile translationFile in translationFiles) {
            this.TryToLoadTranslationFile(translationFile, isReload);
        }
    }

    private void TryToLoadTranslationFile(TranslationFile translationFile, bool isReload) {
        try {
            translationFile.Init();
            if (!translationFile.IsOK) {
                Mod.Logger.LogError(typeof(TranslationFileService),
                                    this.FailedToLoad,
                                    [translationFile]);
                return;
            }
            if (!isReload) {
                this.ModLocales.Add(translationFile);
                Mod.Logger.LogInfo(typeof(TranslationFileService),
                                   this.SeeWhatItIs,
                                   [translationFile]);
                this.TryToAddLocale(translationFile);
            }
            this.TryToAddSource(translationFile);
        } catch (Exception ex) {
            Mod.Logger.LogError(typeof(TranslationFileService),
                                this.FailedToLoad,
                                [translationFile, ex]);
        }
    }

    private void TryToAddSource(TranslationFile translationFile) {
        try {
            LocalizationManager.AddSource(translationFile.LocaleId,
                                          translationFile);
        } catch {
            Mod.Logger.LogError(typeof(TranslationFileService),
                                this.FailedToLoad,
                                [translationFile]);
            this.TryToUnload(translationFile);
            throw;
        }
    }

    private void TryToAddLocale(TranslationFile translationFile) {
        try {
            LocalizationManager.AddLocale(translationFile.LocaleId,
                                          (SystemLanguage) translationFile.Language,
                                          translationFile.LocaleName);
        } catch {
            Mod.Logger.LogError(typeof(TranslationFileService),
                                this.FailedToLoad,
                                [translationFile]);
            LocalizationManager.RemoveLocale(translationFile.LocaleId);
            throw;
        }
    }

    public void Load(bool isReload = false) {
        IList<TranslationFile> translationFiles = this.GetFiles();
        //
        // to assign the 'correct' SystemLanguage and block them
        /// <seealso cref="ModLocale.ModLocale(String, String)"/>
        /// <seealso cref="SystemLanguageHelper.IsRandomizeLanguage(SystemLanguage?)"/>
        /// <seealso cref="SystemLanguageHelper.Random"/>
        IList<TranslationFile> loadFirst = this.WithoutBuiltIn(translationFiles);
        this.LoadFiles(loadFirst, isReload);
        if (false) {
            // TODO: this is crap
            //
            // to assign a remaining SystemLanguage
            /// <seealso cref="ModLocale.ModLocale(String, String)"/>
            /// <seealso cref="SystemLanguageHelper.IsRandomizeLanguage(SystemLanguage?)"/>
            /// <seealso cref="SystemLanguageHelper.Random"/>
            IList<TranslationFile> loadLast = this.OnlyBuiltIn(translationFiles);
            this.LoadFiles(loadLast, isReload);
        }
    }

    public void Reload() {
        // TODO:
        if (false) {
            // INFO: it seems to be impossible to unload...
            this.Unload();
        }
        this.Load(true);

    }

    private void Unload() {
        foreach (TranslationFile translationFile in this.ModLocales) {
            this.TryToUnload(translationFile);
        }
    }

    private void TryToUnload(TranslationFile translationFile) {
        try {
            LocalizationManager.RemoveSource(translationFile.LocaleId,
                                             translationFile);
            LocalizationManager.RemoveLocale(translationFile.LocaleId);
        } catch (Exception ex) {
            Mod.Logger.LogError(typeof(TranslationFileService),
                                this.FailedToUnLoad,
                                [ex, translationFile]);
        }
    }
}
