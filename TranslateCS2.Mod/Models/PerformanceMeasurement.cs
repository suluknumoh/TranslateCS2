using System.Diagnostics;

using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Loggers;

namespace TranslateCS2.Mod.Models;
internal class PerformanceMeasurement {
    private readonly ModRuntimeContainerHandler runtimeContainerHandler;
    private readonly bool measure;
    private readonly MyLanguages languages;
    private readonly Stopwatch stopwatch = new Stopwatch();
    public PerformanceMeasurement(ModRuntimeContainerHandler runtimeContainerHandler, bool measure) {
        this.runtimeContainerHandler = runtimeContainerHandler;
        this.measure = measure;
        this.languages = runtimeContainerHandler.RuntimeContainer.Languages;
    }
    public void Start() {
        if (this.measure) {
            this.stopwatch.Start();
        }
    }
    public void Stop() {
        if (this.measure) {
            this.stopwatch.Stop();
            object[] messageParameters = [
                "it took",
                this.stopwatch.Elapsed,
                "to read and load",
                this.languages.LanguageCount,
                "languages with a total of",
                this.languages.FlavorCountOfAllLanguages,
                "flavors/files with a total of",
                this.languages.EntryCountOfAllFlavorsOfAllLanguages.ToString("N0"),
                "entries"
            ];
            this.runtimeContainerHandler.RuntimeContainer.Logger?.LogInfo(this.GetType(),
                                                                          "performance measurement",
                                                                          messageParameters);
        }
    }
}
