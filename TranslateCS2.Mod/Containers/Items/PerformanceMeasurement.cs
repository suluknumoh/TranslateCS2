using System.Diagnostics;

using TranslateCS2.Inf.Attributes;

namespace TranslateCS2.Mod.Containers.Items;
[MyExcludeFromCoverage]
internal class PerformanceMeasurement {
    private readonly IModRuntimeContainer runtimeContainer;
    private readonly bool measure = false;
    private readonly Stopwatch stopwatch = new Stopwatch();
    public PerformanceMeasurement(IModRuntimeContainer runtimeContainer) {
        this.runtimeContainer = runtimeContainer;
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
                this.runtimeContainer.Languages.LanguageCount,
                "languages with a total of",
                this.runtimeContainer.Languages.FlavorCountOfAllLanguages,
                "flavors/files with a total of",
                this.runtimeContainer.Languages.EntryCountOfAllFlavorsOfAllLanguages.ToString("N0"),
                "entries"
            ];
            this.runtimeContainer.Logger.LogInfo(this.GetType(),
                                                 "performance measurement",
                                                 messageParameters);
        }
    }
}
