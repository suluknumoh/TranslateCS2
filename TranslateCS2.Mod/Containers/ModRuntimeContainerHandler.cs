using System.Threading;

using Colossal.Logging;

using Game.SceneFlow;

using TranslateCS2.Inf.Loggers;
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
                    ModRuntimeContainer runtimeContainer = new ModRuntimeContainer(gameManager, logProvider);
                    Instance = new ModRuntimeContainerHandler(runtimeContainer);
                }
            } finally {
                semaphoreSlim.Release();
            }
        }
    }
}
