using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

using Colossal.IO.AssetDatabase;

using Game.SceneFlow;

using TranslateCS2.Mod.Loggers;
using TranslateCS2.Mod.Models;

using UnityEngine;

namespace TranslateCS2.Mod.Helpers;
internal static class TranslationFileHelper {
    private static string SeeWhatItIs => "just to see, what it is:\r\n{0}";
    private static string FailedToLoad => "failed to load: {0}\r\n{1}";
    private static string JsonExtension => ".json";
    private static string SearchPattern => $"*{JsonExtension}";
    public static IList<TranslationFile> GetFiles(ExecutableAsset asset) {
        List<TranslationFile> files = [];
        string assetDirectoryPath = asset.path.Replace($"{Mod.Name}{ExecutableAsset.kExtension}", String.Empty);
        IEnumerable<string> translationFilePaths = Directory.EnumerateFiles(assetDirectoryPath, SearchPattern);
        foreach (string translationFilePath in translationFilePaths) {
            string name = translationFilePath
                .Replace(assetDirectoryPath, String.Empty)
                .Replace(JsonExtension, String.Empty);
            TranslationFile translationFile = new TranslationFile(name, translationFilePath);
            files.Add(translationFile);
        }
        return files;
    }

    public static IList<TranslationFile> OnlyBuiltIn(IList<TranslationFile> translationFiles) {
        return translationFiles.Where(translationFile => ModLocale.BuiltIn.Contains(translationFile.Name)).ToList();
    }

    public static IList<TranslationFile> WithoutBuiltIn(IList<TranslationFile> translationFiles) {
        return translationFiles.Where(translationFile => !ModLocale.BuiltIn.Contains(translationFile.Name)).ToList();
    }
    public static void LoadFiles(IList<TranslationFile> translationFiles) {
        foreach (TranslationFile translationFile in translationFiles) {
            TryToLoadTranslationFile(translationFile);
        }
    }

    private static void TryToLoadTranslationFile(TranslationFile translationFile) {
        try {
            ModLocale modLocale = ModLocale.Read(translationFile);
            if (!modLocale.IsOK) {
                Mod.Logger.LogError(typeof(TranslationFileHelper),
                                    FailedToLoad,
                                    [translationFile, modLocale]);
                return;
            }
            Mod.Logger.LogInfo(typeof(TranslationFileHelper),
                               SeeWhatItIs,
                               [modLocale]);
            TryToAddLocale(modLocale, translationFile);
            TryToAddSource(modLocale, translationFile);
        } catch (Exception ex) {
            Mod.Logger.LogError(typeof(TranslationFileHelper),
                                FailedToLoad,
                                [translationFile, ex]);
        }
    }

    private static void TryToAddSource(ModLocale modLocale, TranslationFile translationFile) {
        try {
            GameManager.instance.localizationManager.AddSource(modLocale.LocaleId,
                                                               modLocale);
        } catch {
            Mod.Logger.LogError(typeof(TranslationFileHelper),
                                FailedToLoad,
                                [translationFile, modLocale]);
            GameManager.instance.localizationManager.RemoveLocale(modLocale.LocaleId);
            GameManager.instance.localizationManager.RemoveSource(modLocale.LocaleId,
                                                                  modLocale);
            throw;
        }
    }

    private static void TryToAddLocale(ModLocale modLocale, TranslationFile translationFile) {
        try {
            GameManager.instance.localizationManager.AddLocale(modLocale.LocaleId,
                                                               (SystemLanguage) modLocale.Language,
                                                               modLocale.LocaleName);
        } catch {
            Mod.Logger.LogError(typeof(TranslationFileHelper),
                                FailedToLoad,
                                [translationFile, modLocale]);
            GameManager.instance.localizationManager.RemoveLocale(modLocale.LocaleId);
            throw;
        }
    }
}
