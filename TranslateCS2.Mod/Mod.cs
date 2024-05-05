using System;

using Colossal.IO.AssetDatabase;
using Colossal.Logging;

using Game;
using Game.Modding;
using Game.SceneFlow;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Loggers;
using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod;
public class Mod : IMod {
    private static ILog Logger { get; } = LogManager.GetLogger(ModConstants.Name).SetShowsErrorsInUI(false);
    private ModRuntimeContainerHandler runtimeContainerHandler;
    private ModSettings? _modSettings;
    public Mod() { }
    public void OnLoad(UpdateSystem updateSystem) {
        try {
            Logger.LogInfo(this.GetType(), nameof(OnLoad));
            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out ExecutableAsset asset)) {
                ModRuntimeContainer runtimeContainer = new ModRuntimeContainer(GameManager.instance, Logger);
                ModRuntimeContainerHandler.Init(runtimeContainer);
                this.runtimeContainerHandler = ModRuntimeContainerHandler.Instance;
                MyLanguages languages = runtimeContainer.Languages;
                //
                //
                // cant be controlled via settings,
                // cause they have to be loaded after files are read and loaded
                PerformanceMeasurement performanceMeasurement = new PerformanceMeasurement(this.runtimeContainerHandler, false);
                performanceMeasurement.Start();
                //
                languages.ReadFiles();
                languages.Load();
                //
                performanceMeasurement.Stop();
                //
                //
                if (languages.HasErroneous) {
                    runtimeContainer.ErrorMessages.DisplayErrorMessageForErroneous(languages.Erroneous, false);
                }
                this._modSettings = new ModSettings(this.runtimeContainerHandler, this);
                ModSettingsLocale modSettingsLocale = new ModSettingsLocale(this._modSettings,
                                                                            this.runtimeContainerHandler);
                this._modSettings.OnFlavorChanged += languages.FlavorChanged;
                this._modSettings.RegisterInOptionsUI();
                // settings have to be loaded after files are read and loaded
                AssetDatabase.global.LoadSettings(ModConstants.Name, this._modSettings);
                runtimeContainer.LocManager?.AddSource(runtimeContainer.LocManager.fallbackLocaleId,
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
