using System;
using System.Collections.Generic;

using Colossal.IO.AssetDatabase;
using Colossal.Logging;

using Game;
using Game.Modding;
using Game.SceneFlow;

using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Loggers;
using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod;
public class Mod : IMod {
    public const string Name = $"{nameof(TranslateCS2)}.{nameof(Mod)}";
    public static ILog Logger = LogManager.GetLogger(Name).SetShowsErrorsInUI(false);
    private string StrangerThings => "failed to load the entire mod: {0}";

    public void OnLoad(UpdateSystem updateSystem) {
        try {
            Mod.Logger.LogInfo(this.GetType(), nameof(OnLoad));
            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out ExecutableAsset asset)) {
                IList<TranslationFile> translationFiles = TranslationFileHelper.GetFiles(asset);
                //
                // to assign the 'correct' SystemLanguage and block them
                /// <seealso cref="ModLocale.ModLocale(String, String)"/>
                /// <seealso cref="SystemLanguageHelper.IsRandomizeLanguage(SystemLanguage?)"/>
                /// <seealso cref="SystemLanguageHelper.Random"/>
                IList<TranslationFile> loadFirst = TranslationFileHelper.WithoutBuiltIn(translationFiles);
                TranslationFileHelper.LoadFiles(loadFirst);
                //
                // to assign a remaining SystemLanguage
                /// <seealso cref="ModLocale.ModLocale(String, String)"/>
                /// <seealso cref="SystemLanguageHelper.IsRandomizeLanguage(SystemLanguage?)"/>
                /// <seealso cref="SystemLanguageHelper.Random"/>
                IList<TranslationFile> loadLast = TranslationFileHelper.OnlyBuiltIn(translationFiles);
                TranslationFileHelper.LoadFiles(loadLast);
                //
                Setting setting = new Setting(this);
                AssetDatabase.global.LoadSettings(Name, setting, setting);
            }
        } catch (Exception ex) {
            Mod.Logger.LogCritical(this.GetType(),
                                   this.StrangerThings,
                                   [ex]);
        }
    }

    public void OnDispose() {
        Mod.Logger.LogInfo(this.GetType(), nameof(OnDispose));
    }
}
