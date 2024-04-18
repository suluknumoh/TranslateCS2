using System;
using System.Collections.Generic;
using System.IO;

using Colossal.IO.AssetDatabase;
using Colossal.Logging;

using Game;
using Game.Modding;
using Game.SceneFlow;

using TranslateCS2.Mod.Models;

using UnityEngine;

namespace TranslateCS2.Mod;
public class Mod : IMod {
    public const string Name = $"{nameof(TranslateCS2)}.{nameof(Mod)}";
    public static ILog log = LogManager.GetLogger(Name).SetShowsErrorsInUI(false);

    public void OnLoad(UpdateSystem updateSystem) {
        Mod.log.Info(nameof(OnLoad));
        if (GameManager.instance.modManager.TryGetExecutableAsset(this, out ExecutableAsset asset)) {
            IEnumerable<string> translationFiles = Directory.EnumerateFiles(asset.path.Replace($"{Name}.dll", String.Empty), "*.json");
            Setting setting = new Setting(this);
            foreach (string translationFile in translationFiles) {
                try {
                    ModLocale modLocale = ModLocale.Read(translationFile);
                    if (!modLocale.IsOK) {
                        Mod.log.Error($"failed to load: {translationFile}");
                        continue;
                    }
                    GameManager.instance.localizationManager.AddLocale(modLocale.LocaleId, (SystemLanguage) modLocale.Language, modLocale.LocaleName);
                    GameManager.instance.localizationManager.AddSource(modLocale.LocaleId, modLocale);
                } catch {
                    Mod.log.Error($"failed to load: {translationFile}");
                }
            }
            AssetDatabase.global.LoadSettings(Name, setting, setting);
        }
    }
    public void OnDispose() {
        //
    }
}
