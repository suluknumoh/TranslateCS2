using System.Threading;

namespace TranslateCS2.Mod.Containers;
public class ModRuntimeContainerHandler {
    private static readonly SemaphoreSlim semaphoreSlim = new SemaphoreSlim(1, 1);
    public static ModRuntimeContainerHandler Instance { get; private set; }
    public IModRuntimeContainer RuntimeContainer { get; }
    private ModRuntimeContainerHandler(IModRuntimeContainer runtimeContainer) {
        this.RuntimeContainer = runtimeContainer;
    }
    public static void Init(IModRuntimeContainer runtimeContainer) {
        if (Instance == null) {
            semaphoreSlim.Wait();
            try {
                Instance ??= new ModRuntimeContainerHandler(runtimeContainer);
            } finally {
                semaphoreSlim.Release();
            }
        }
    }
}
