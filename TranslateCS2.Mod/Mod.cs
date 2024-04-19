using System;
using System.Collections.Generic;
using System.IO;

using Colossal.IO.AssetDatabase;
using Colossal.Logging;

using Game;
using Game.Modding;
using Game.SceneFlow;

using TranslateCS2.Mod.Loggers;
using TranslateCS2.Mod.Models;

using UnityEngine;

namespace TranslateCS2.Mod;
public class Mod : IMod {
    public const string Name = $"{nameof(TranslateCS2)}.{nameof(Mod)}";
    public static ILog log = LogManager.GetLogger(Name).SetShowsErrorsInUI(false);

    private string FailedToLoad => "failed to load: {0}\r\n{1}";
    private string StrangerThings => "failed to load the entire mod: {0}";

    private string JsonExtension => "*.json";

    public void OnLoad(UpdateSystem updateSystem) {
        try {
            Mod.log.LogInfo(this.GetType(), nameof(OnLoad));
            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out ExecutableAsset asset)) {

                string assetDirectoryPath = asset.path.Replace($"{Name}{ExecutableAsset.kExtension}", String.Empty);
                IEnumerable<string> translationFiles = Directory.EnumerateFiles(assetDirectoryPath, this.JsonExtension);
                foreach (string translationFile in translationFiles) {
                    this.TryToLoadTranslationFile(translationFile);
                }
                Setting setting = new Setting(this);
                AssetDatabase.global.LoadSettings(Name, setting, setting);
            }
        } catch (Exception ex) {
            Mod.log.LogCritical(this.GetType(),
                                this.StrangerThings,
                                [ex]);
        }
    }

    private void TryToLoadTranslationFile(string translationFile) {
        try {
            ModLocale modLocale = ModLocale.Read(translationFile);
            if (!modLocale.IsOK) {
                Mod.log.LogError(this.GetType(),
                                 this.FailedToLoad,
                                 [translationFile, modLocale]);
                return;
            }
            this.TryToAddLocale(modLocale, translationFile);
            this.TryToAddSource(modLocale, translationFile);
        } catch (Exception ex) {
            Mod.log.LogError(this.GetType(),
                             this.FailedToLoad,
                             [translationFile, ex]);
        }
    }

    private void TryToAddSource(ModLocale modLocale, string translationFile) {
        try {
            GameManager.instance.localizationManager.AddSource(modLocale.LocaleId,
                                                               modLocale);
        } catch {
            Mod.log.LogError(this.GetType(),
                             this.FailedToLoad,
                             [translationFile, modLocale]);
            GameManager.instance.localizationManager.RemoveLocale(modLocale.LocaleId);
            GameManager.instance.localizationManager.RemoveSource(modLocale.LocaleId,
                                                                  modLocale);
            throw;
        }
    }

    private void TryToAddLocale(ModLocale modLocale, string translationFile) {
        try {
            GameManager.instance.localizationManager.AddLocale(modLocale.LocaleId,
                                                               (SystemLanguage) modLocale.Language,
                                                               modLocale.LocaleName);
        } catch {
            Mod.log.LogError(this.GetType(),
                             this.FailedToLoad,
                             [translationFile, modLocale]);
            GameManager.instance.localizationManager.RemoveLocale(modLocale.LocaleId);
            throw;
        }
    }

    public void OnDispose() {
        Mod.log.LogInfo(this.GetType(), nameof(OnDispose));
    }
}
