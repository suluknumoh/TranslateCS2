using System.Threading;

using Colossal.IO.AssetDatabase;
using Colossal.Logging;
using Colossal.PSI.Environment;

using Game.SceneFlow;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Loggers;
using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Mod.Containers.Items.Unitys;
using TranslateCS2.Mod.Interfaces;
using TranslateCS2.Mod.Loggers;

namespace TranslateCS2.Mod.Containers;
internal class ModRuntimeContainerHandler {
    private static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
    public static ModRuntimeContainerHandler Instance { get; private set; }
    public IModRuntimeContainer RuntimeContainer { get; }
    private ModRuntimeContainerHandler(IModRuntimeContainer runtimeContainer) {
        this.RuntimeContainer = runtimeContainer;
    }
    public static void Init(ILog logger, GameManager gameManager) {
        if (Instance is null) {
            try {
                semaphoreSlim.Wait();
                if (Instance is null) {
                    IMyLogProvider logProvider = new ModLogProvider(logger);
                    Paths paths = new Paths(true,
                                            EnvPath.kStreamingDataPath,
                                            EnvPath.kUserDataPath);
                    ILocManager locManager = new LocManager(gameManager.localizationManager);
                    IIntSettings intSettings = new IntSettings(gameManager.settings.userInterface);
                    IIndexCountsProvider indexCountsProvider = new IndexCountsProvider(AssetDatabase.global);
                    ModRuntimeContainer runtimeContainer = new ModRuntimeContainer(logProvider,
                                                                                   locManager,
                                                                                   intSettings,
                                                                                   indexCountsProvider,
                                                                                   paths);
                    Instance = new ModRuntimeContainerHandler(runtimeContainer);
                }
            } finally {
                semaphoreSlim.Release();
            }
        }
    }
}
