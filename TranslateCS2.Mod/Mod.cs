using System;

using Colossal.IO.AssetDatabase;
using Colossal.Logging;

using Game;
using Game.Modding;
using Game.SceneFlow;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Loggers;
using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod;
public class Mod : IMod {
    private static ILog Logger { get; } = LogManager.GetLogger(ModConstants.Name).SetShowsErrorsInUI(false);
    private IModRuntimeContainer runtimeContainer;
    private MyLanguages languages;

    private ModSettings? _modSettings;
    public Mod() { }
    public void OnLoad(UpdateSystem updateSystem) {
        try {
            Logger.LogInfo(this.GetType(), nameof(OnLoad));
            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out ExecutableAsset asset)) {
                this.runtimeContainer = new ModRuntimeContainer(GameManager.instance, Logger);
                this.languages = MyLanguages.GetInstance(this.runtimeContainer);
                //
                //
                // cant be controlled via settings,
                // cause they have to be loaded after files are read and loaded
                PerformanceMeasurement performanceMeasurement = new PerformanceMeasurement(this.runtimeContainer, this.languages, false);
                performanceMeasurement.Start();
                //
                this.languages.ReadFiles();
                this.languages.Load();
                //
                performanceMeasurement.Stop();
                //
                //
                if (this.languages.HasErroneous) {
                    this.runtimeContainer.ErrorMessageHelper.DisplayErrorMessageForErroneous(this.languages.Erroneous, false);
                }
                this._modSettings = new ModSettings(this.runtimeContainer, this.languages, this);
                ModSettingsLocale modSettingsLocale = new ModSettingsLocale(this._modSettings,
                                                                            this.runtimeContainer,
                                                                            this.languages);
                this._modSettings.OnFlavorChanged += this.languages.FlavorChanged;
                this._modSettings.RegisterInOptionsUI();
                // settings have to be loaded after files are read and loaded
                AssetDatabase.global.LoadSettings(ModConstants.Name, this._modSettings);
                this.runtimeContainer.LocManager?.AddSource(this.runtimeContainer.LocManager.fallbackLocaleId,
                                                            modSettingsLocale);
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
