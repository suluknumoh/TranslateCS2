using System;

using Colossal.IO.AssetDatabase;
using Colossal.Localization;
using Colossal.Logging;

using Game;
using Game.Modding;
using Game.SceneFlow;

using TranslateCS2.Mod.Loggers;
using TranslateCS2.Mod.Models;
using TranslateCS2.ModBridge;

namespace TranslateCS2.Mod;
public class Mod : IMod {
    public static ILog Logger { get; } = LogManager.GetLogger(ModConstants.Name).SetShowsErrorsInUI(false);
    private static GameManager GameManager { get; } = GameManager.instance;
    private static MyCountrys Countrys { get; } = MyCountrys.Instance;
    private static LocalizationManager LocalizationManager { get; } = Mod.GameManager.localizationManager;

    private ModSettings? _setting;

    public void OnLoad(UpdateSystem updateSystem) {
        try {
            Logger.LogInfo(this.GetType(), nameof(OnLoad));
            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out ExecutableAsset asset)) {
                //
                //
                Countrys.ReadFiles();
                //
                //
                this._setting = new ModSettings(this);
                this._setting.RegisterInOptionsUI();
                AssetDatabase.global.LoadSettings(ModConstants.Name, this._setting);
                Mod.LocalizationManager.AddSource(LocalizationManager.fallbackLocaleId, new ModSettingsLocale(this._setting));
                this._setting.HandleLocaleOnLoad();
            }
        } catch (Exception ex) {
            Logger.LogCritical(this.GetType(),
                               ModConstants.StrangerThings,
                               [ex]);
        }
    }

    public void OnDispose() {
        try {
            Logger.LogInfo(this.GetType(), nameof(OnDispose));
            this._setting?.UnregisterInOptionsUI();
            this._setting?.HandleLocaleOnUnLoad();
        } catch (Exception ex) {
            Logger.LogCritical(this.GetType(),
                               ModConstants.StrangerThingsDispose,
                               [ex]);
        }
    }
}
