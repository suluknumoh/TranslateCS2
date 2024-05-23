using System.Diagnostics;

using TranslateCS2.Inf.Attributes;
using TranslateCS2.Mod.Containers;
using TranslateCS2.Mod.Containers.Items;

namespace TranslateCS2.Mod.Models;
[MyExcludeFromCoverage]
internal class PerformanceMeasurement {
    private readonly IModRuntimeContainer runtimeContainer;
    private readonly bool measure;
    private readonly MyLanguages languages;
    private readonly Stopwatch stopwatch = new Stopwatch();
    public PerformanceMeasurement(IModRuntimeContainer runtimeContainer, bool measure) {
        this.runtimeContainer = runtimeContainer;
        this.measure = measure;
        this.languages = runtimeContainer.Languages;
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
            this.runtimeContainer.Logger?.LogInfo(this.GetType(),
                                                  "performance measurement",
                                                  messageParameters);
        }
    }
}
