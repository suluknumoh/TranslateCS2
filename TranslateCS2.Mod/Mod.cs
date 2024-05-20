using System;

using Colossal.IO.AssetDatabase;
using Colossal.Logging;

using Game;
using Game.Modding;
using Game.SceneFlow;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Loggers;
using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod;
public class Mod : IMod {
    private static readonly ILog Logger = LogManager.GetLogger(ModConstants.Name).SetShowsErrorsInUI(false);
    private ModRuntimeContainerHandler runtimeContainerHandler;
    private ModSettings? modSettings;
    public Mod() { }
    public void OnLoad(UpdateSystem updateSystem) {
        try {
            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out ExecutableAsset asset)) {
                IMyLogProvider logProvider = new ModLogProvider(Logger);
                ModRuntimeContainer runtimeContainer = new ModRuntimeContainer(GameManager.instance, logProvider);
                runtimeContainer.Logger.LogInfo(this.GetType(), nameof(OnLoad));
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
                this.modSettings = new ModSettings(this.runtimeContainerHandler, this);
                ModSettingsLocale modSettingsLocale = new ModSettingsLocale(this.modSettings,
                                                                            this.runtimeContainerHandler);
                this.modSettings.RegisterInOptionsUI();
                // settings have to be loaded after files are read and loaded
                AssetDatabase.global.LoadSettings(ModConstants.Name, this.modSettings);
                runtimeContainer.LocManager?.AddSource(runtimeContainer.LocManager.fallbackLocaleId,
                                                       modSettingsLocale);
                this.modSettings.HandleLocaleOnLoad();
            }
        } catch (Exception ex) {
            // user LogManagers Logger
            // runtimeContainerHandler might not be initialized
            Logger.Critical(ex);
        }
    }

    public void OnDispose() {
        IMyLogger logger = this.runtimeContainerHandler.RuntimeContainer.Logger;
        try {
            logger.LogInfo(this.GetType(), nameof(OnDispose));
            this.modSettings?.UnregisterInOptionsUI();
            this.modSettings?.HandleLocaleOnUnLoad();
        } catch (Exception ex) {
            logger.LogCritical(this.GetType(),
                               LoggingConstants.StrangerThingsDispose,
                               [ex]);
        }
    }
}
