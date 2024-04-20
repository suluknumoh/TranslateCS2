using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Colossal.IO.AssetDatabase;

using Game.SceneFlow;

using TranslateCS2.Mod.Loggers;
using TranslateCS2.Mod.Models;

using UnityEngine;

namespace TranslateCS2.Mod.Services;
internal class TranslationFileService {
    private string SeeWhatItIs => "just to see, what it is:\r\n{0}";
    private string FailedToLoad => "failed to load: {0}\r\n{1}";
    private string FailedToUnLoad => "failed to unload:\r\n{0}";
    private string JsonExtension => ".json";
    private string SearchPattern => $"*{this.JsonExtension}";
    private IList<ModLocale> ModLocales { get; } = [];
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
        return translationFiles.Where(translationFile => ModLocale.BuiltIn.Contains(translationFile.Name)).ToList();
    }

    private IList<TranslationFile> WithoutBuiltIn(IList<TranslationFile> translationFiles) {
        return translationFiles.Where(translationFile => !ModLocale.BuiltIn.Contains(translationFile.Name)).ToList();
    }
    private void LoadFiles(IList<TranslationFile> translationFiles) {
        foreach (TranslationFile translationFile in translationFiles) {
            this.TryToLoadTranslationFile(translationFile);
        }
    }

    private void TryToLoadTranslationFile(TranslationFile translationFile) {
        try {
            ModLocale modLocale = ModLocale.Read(translationFile);
            if (!modLocale.IsOK) {
                Mod.Logger.LogError(typeof(TranslationFileService),
                                    this.FailedToLoad,
                                    [translationFile, modLocale]);
                return;
            }
            this.ModLocales.Add(modLocale);
            Mod.Logger.LogInfo(typeof(TranslationFileService),
                               this.SeeWhatItIs,
                               [modLocale]);
            this.TryToAddLocale(modLocale, translationFile);
            this.TryToAddSource(modLocale, translationFile);
        } catch (Exception ex) {
            Mod.Logger.LogError(typeof(TranslationFileService),
                                this.FailedToLoad,
                                [translationFile, ex]);
        }
    }

    private void TryToAddSource(ModLocale modLocale, TranslationFile translationFile) {
        try {
            GameManager.instance.localizationManager.AddSource(modLocale.LocaleId,
                                                               modLocale);
        } catch {
            Mod.Logger.LogError(typeof(TranslationFileService),
                                this.FailedToLoad,
                                [translationFile, modLocale]);
            this.TryToUnload(modLocale);
            throw;
        }
    }

    private void TryToAddLocale(ModLocale modLocale, TranslationFile translationFile) {
        try {
            GameManager.instance.localizationManager.AddLocale(modLocale.LocaleId,
                                                               (SystemLanguage) modLocale.Language,
                                                               modLocale.LocaleName);
        } catch {
            Mod.Logger.LogError(typeof(TranslationFileService),
                                this.FailedToLoad,
                                [translationFile, modLocale]);
            GameManager.instance.localizationManager.RemoveLocale(modLocale.LocaleId);
            throw;
        }
    }

    public void Load() {
        IList<TranslationFile> translationFiles = this.GetFiles();
        //
        // to assign the 'correct' SystemLanguage and block them
        /// <seealso cref="ModLocale.ModLocale(String, String)"/>
        /// <seealso cref="SystemLanguageHelper.IsRandomizeLanguage(SystemLanguage?)"/>
        /// <seealso cref="SystemLanguageHelper.Random"/>
        IList<TranslationFile> loadFirst = this.WithoutBuiltIn(translationFiles);
        this.LoadFiles(loadFirst);
        if (false) {
            // TODO: this is crap
            //
            // to assign a remaining SystemLanguage
            /// <seealso cref="ModLocale.ModLocale(String, String)"/>
            /// <seealso cref="SystemLanguageHelper.IsRandomizeLanguage(SystemLanguage?)"/>
            /// <seealso cref="SystemLanguageHelper.Random"/>
            IList<TranslationFile> loadLast = this.OnlyBuiltIn(translationFiles);
            this.LoadFiles(loadLast);
        }
    }

    public void Reload() {
        // TODO:
        if (false) {
            if (false) {
                this.Unload();
            }
            this.Load();
        }
    }

    private void Unload() {
        foreach (ModLocale modLocale in this.ModLocales) {
            this.TryToUnload(modLocale);
        }
    }

    private void TryToUnload(ModLocale modLocale) {
        try {
            GameManager.instance.localizationManager.RemoveSource(modLocale.LocaleId,
                                                                  modLocale);
            GameManager.instance.localizationManager.RemoveLocale(modLocale.LocaleId);
        } catch (Exception ex) {

            Mod.Logger.LogError(typeof(TranslationFileService),
                                this.FailedToUnLoad,
                                [modLocale]);
        }
    }
}
