using System.IO;

using TranslateCS2.Core.Configurations;

namespace TranslateCS2.Core.Services.InstallPaths;
internal abstract class APathDetector : IInstallPathDetector {
    private readonly string citiesExeName = AppConfigurationManager.CitiesExe;
    public string? InstallPath { get; protected set; }
    public APathDetector() { }
    public virtual bool Detect() {
        return this.Exists();
    }
    private bool Exists() {
        if (this.InstallPath is null) {
            return false;
        }
        string exePath = Path.Combine(this.InstallPath, this.citiesExeName);
        return File.Exists(exePath);
    }
}
