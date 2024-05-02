using System.Diagnostics;

using TranslateCS2.Mod.Loggers;

namespace TranslateCS2.Mod.Models;
internal class PerformanceMeasurement {
    private readonly bool _measure;
    private readonly MyLanguages _languages = MyLanguages.Instance;
    private readonly Stopwatch _stopwatch = new Stopwatch();
    public PerformanceMeasurement(bool measure) {
        this._measure = measure;
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
            Mod.Logger.LogInfo(this.GetType(),
                               "performance measurement",
                               messageParameters);
        }
    }
}
