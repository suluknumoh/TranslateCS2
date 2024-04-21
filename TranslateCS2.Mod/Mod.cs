using System;

using Colossal.IO.AssetDatabase;
using Colossal.Localization;
using Colossal.Logging;

using Game;
using Game.Modding;
using Game.SceneFlow;

using TranslateCS2.Mod.Loggers;
using TranslateCS2.Mod.Models;
using TranslateCS2.Mod.Services;

namespace TranslateCS2.Mod;
public class Mod : IMod {
    public const string Name = $"{nameof(TranslateCS2)}.{nameof(Mod)}";
    public static string NameSimple { get; } = nameof(TranslateCS2);
    public static ILog Logger { get; } = LogManager.GetLogger(Name).SetShowsErrorsInUI(false);
    private static GameManager GameManager { get; } = GameManager.instance;
    private static LocalizationManager LocalizationManager { get; } = Mod.GameManager.localizationManager;
    private string StrangerThings => "failed to load the entire mod:";
    private ModSettings? _setting;
    private TranslationFileService? _translationFileService;

    public void OnLoad(UpdateSystem updateSystem) {
        try {
            Logger.LogInfo(this.GetType(), nameof(OnLoad));
            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out ExecutableAsset asset)) {
                this._translationFileService = new TranslationFileService(asset);
                this._setting = new ModSettings(this, this._translationFileService);
                this._setting.RegisterInOptionsUI();
                AssetDatabase.global.LoadSettings(Name, this._setting);
                Mod.LocalizationManager.AddSource("en-US", new ModSettingsLocale(this._setting));
                //
                //
                this._translationFileService.Load();
                //
                //
                this._setting.HandleLocaleOnLoad();
            }
        } catch (Exception ex) {
            Logger.LogCritical(this.GetType(),
                               this.StrangerThings,
                               [ex]);
        }
    }

    public void OnDispose() {
        Logger.LogInfo(this.GetType(), nameof(OnDispose));
        this._setting?.UnregisterInOptionsUI();
        this._setting?.HandleLocaleOnUnLoad();
    }
}
