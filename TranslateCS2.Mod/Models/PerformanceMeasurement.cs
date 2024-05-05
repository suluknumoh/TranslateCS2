using System.Diagnostics;

using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.Mod.Loggers;

namespace TranslateCS2.Mod.Models;
internal class PerformanceMeasurement {
    private readonly ModRuntimeContainerHandler runtimeContainerHandler;
    private readonly bool _measure;
    private readonly MyLanguages _languages;
    private readonly Stopwatch _stopwatch = new Stopwatch();
    public PerformanceMeasurement(ModRuntimeContainerHandler runtimeContainerHandler, bool measure) {
        this.runtimeContainerHandler = runtimeContainerHandler;
        this._measure = measure;
        this._languages = runtimeContainerHandler.RuntimeContainer.Languages;
    }
    public void Start() {
        if (this._measure) {
            this._stopwatch.Start();
        }
    }
    public void Stop() {
        if (this._measure) {
            this._stopwatch.Stop();
            object[] messageParameters = [
                "it took",
                this._stopwatch.Elapsed,
                "to read and load",
                this._languages.LanguageCount,
                "languages with a total of",
                this._languages.FlavorCountOfAllLanguages,
                "flavors/files with a total of",
                this._languages.EntryCountOfAllFlavorsOfAllLanguages.ToString("N0"),
                "entries"
            ];
            this.runtimeContainerHandler.RuntimeContainer.Logger?.LogInfo(this.GetType(),
                                                                          "performance measurement",
                                                                          messageParameters);
        }
    }
}
