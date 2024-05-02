using System;

using Colossal.IO.AssetDatabase;
using Colossal.Localization;
using Colossal.Logging;

using Game;
using Game.Modding;
using Game.SceneFlow;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Helpers;
using TranslateCS2.Mod.Loggers;
using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod;
public class Mod : IMod {
    public static ILog Logger { get; } = LogManager.GetLogger(ModConstants.Name).SetShowsErrorsInUI(false);
    private static GameManager GameManager { get; } = GameManager.instance;
    private static MyLanguages Languages { get; } = MyLanguages.Instance;
    private static LocalizationManager LocalizationManager { get; } = Mod.GameManager.localizationManager;

    private ModSettings? _modSettings;

    public void OnLoad(UpdateSystem updateSystem) {
        try {
            Logger.LogInfo(this.GetType(), nameof(OnLoad));
            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out ExecutableAsset asset)) {
                FileSystemHelper.CreateIfNotExists();
                //
                //
                // cant be controlled via settings,
                // cause they have to be loaded after files are read and loaded
                PerformanceMeasurement performanceMeasurement = new PerformanceMeasurement(false);
                performanceMeasurement.Start();
                //
                Languages.ReadFiles();
                Languages.Load();
                //
                performanceMeasurement.Stop();
                //
                //
                if (Languages.HasErroneous) {
                    ErrorMessageHelper.DisplayErrorMessage(Languages.Erroneous, false);
                }
                this._modSettings = new ModSettings(this);
                ModSettingsLocale modSettingsLocale = new ModSettingsLocale(this._modSettings);
                this._modSettings.OnFlavorChanged += Languages.FlavorChanged;
                this._modSettings.RegisterInOptionsUI();
                // settings have to be loaded after files are read and loaded
                AssetDatabase.global.LoadSettings(ModConstants.Name, this._modSettings);
                Mod.LocalizationManager.AddSource(LocalizationManager.fallbackLocaleId, modSettingsLocale);
                this._modSettings.HandleLocaleOnLoad();
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
            this._modSettings?.UnregisterInOptionsUI();
            this._modSettings?.HandleLocaleOnUnLoad();
        } catch (Exception ex) {
            Logger.LogCritical(this.GetType(),
                               LoggingConstants.StrangerThingsDispose,
                               [ex]);
        }
    }
}
