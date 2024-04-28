using System;

using Colossal.IO.AssetDatabase;
using Colossal.Localization;
using Colossal.Logging;

using Game;
using Game.Modding;
using Game.SceneFlow;

using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Loggers;
using TranslateCS2.Mod.Models;
using TranslateCS2.ModBridge;

namespace TranslateCS2.Mod;
public class Mod : IMod {
    public static ILog Logger { get; } = LogManager.GetLogger(ModConstants.Name).SetShowsErrorsInUI(false);
    private static GameManager GameManager { get; } = GameManager.instance;
    private static MyLanguages Languages { get; } = MyLanguages.Instance;
    private static LocalizationManager LocalizationManager { get; } = Mod.GameManager.localizationManager;

    private ModSettings? _setting;

    public void OnLoad(UpdateSystem updateSystem) {
        try {
            Logger.LogInfo(this.GetType(), nameof(OnLoad));
            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out ExecutableAsset asset)) {
                FileSystemHelper.CreateIfNotExists();
                //
                //
                // TODO: dont read all files at startup? only the one selected
                // TODO: what if all langs and flavors get preregistered now; but are tried to read later?
                // TODO: if i would do so, would i need my own error-indicator within logs, to log an error as info, so its not shown in the ui, but marked as error within the log?
                // TODO: is it possible to switch show errors?
                // TODO: for example:
                // log the real error with error-location-information
                // Logger.SetShowsErrorsInUI(true); // swtich to true
                //      log 'failed to load' as error, so an appropriate message is shown within the ui?
                // Logger.SetShowsErrorsInUI(false); // switch back to false
                // TODO: an alternative would be: i know the files, that are present, only register them and load the selected ones, then consider the above
                Languages.ReadFiles();
                Languages.Load();
                //
                //
                this._setting = new ModSettings(this);
                this._setting.OnFlavorChanged += Languages.FlavorChanged;
                this._setting.RegisterInOptionsUI();
                AssetDatabase.global.LoadSettings(ModConstants.Name, this._setting);
                Mod.LocalizationManager.AddSource(LocalizationManager.fallbackLocaleId, new ModSettingsLocale(this._setting));
                this._setting.HandleLocaleOnLoad();
            }
        } catch (Exception ex) {
            Logger.LogCritical(this.GetType(),
                               LoggingConstants.StrangerThings,
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
                               LoggingConstants.StrangerThingsDispose,
                               [ex]);
        }
    }
}
