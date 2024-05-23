using System;

using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Colossal.PSI.Environment;

using Game;
using Game.Modding;
using Game.SceneFlow;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Attributes;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Mod.Interfaces;
using TranslateCS2.Mod.Loggers;
using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod.Containers.Items.Unitys;
[MyExcludeFromCoverage]
public class Mod : IMod {
    public static IModRuntimeContainer RuntimeContainer { get; private set; }
    private static readonly ILog Logger = LogManager.GetLogger(ModConstants.Name).SetShowsErrorsInUI(false);
    private ModSettings? modSettings;
    public Mod() { }
    public void OnLoad(UpdateSystem updateSystem) {
        try {
            if (GameManager.instance.modManager.TryGetExecutableAsset(this, out ExecutableAsset asset)) {
                CreateRuntimeContainer(Logger,
                                       this,
                                       GameManager.instance);
                RuntimeContainer.Logger.LogInfo(this.GetType(), nameof(OnLoad));
                MyLanguages languages = RuntimeContainer.Languages;
                //
                //
                // cant be controlled via settings,
                // cause they have to be loaded after files are read and loaded
                PerformanceMeasurement performanceMeasurement = new PerformanceMeasurement(RuntimeContainer, false);
                performanceMeasurement.Start();
                //
                languages.ReadFiles();
                languages.Load();
                //
                performanceMeasurement.Stop();
                //
                //
                if (languages.HasErroneous) {
                    RuntimeContainer.ErrorMessages.DisplayErrorMessageForErroneous(languages.Erroneous, false);
                }
                this.modSettings = new ModSettings(RuntimeContainer);
                ModSettingsLocale modSettingsLocale = new ModSettingsLocale(this.modSettings,
                                                                            RuntimeContainer);
                this.modSettings.RegisterInOptionsUI();
                // settings have to be loaded after files are read and loaded
                AssetDatabase.global.LoadSettings(ModConstants.Name, this.modSettings);
                RuntimeContainer.LocManager.AddSource(RuntimeContainer.LocManager.FallbackLocaleId,
                                                      modSettingsLocale);
                this.modSettings.HandleLocaleOnLoad();
            }
        } catch (Exception ex) {
            // user LogManagers Logger
            // runtimeContainerHandler might not be initialized
            Logger.Critical(ex, nameof(OnLoad));
        }
    }

    public void OnDispose() {
        try {
            IMyLogger? logger = RuntimeContainer.Logger;
            logger?.LogInfo(this.GetType(), nameof(OnDispose));
            this.modSettings?.UnregisterInOptionsUI();
            this.modSettings?.HandleLocaleOnUnLoad();
        } catch (Exception ex) {
            // user LogManagers Logger
            // runtimeContainerHandler might not be initialized
            Logger.Critical(ex, nameof(OnDispose));
        }
    }


    private static void CreateRuntimeContainer(ILog logger, IMod mod, GameManager gameManager) {
        IMyLogProvider logProvider = new ModLogProvider(logger);
        Paths paths = new Paths(true,
                                EnvPath.kStreamingDataPath,
                                EnvPath.kUserDataPath);
        ILocManager locManager = new LocManager(gameManager.localizationManager);
        IIntSettings intSettings = new IntSettings(gameManager.settings.userInterface);
        IIndexCountsProvider indexCountsProvider = new IndexCountsProvider(AssetDatabase.global);
        RuntimeContainer = new ModRuntimeContainer(logProvider,
                                                   mod,
                                                   locManager,
                                                   intSettings,
                                                   indexCountsProvider,
                                                   paths);
    }

}
