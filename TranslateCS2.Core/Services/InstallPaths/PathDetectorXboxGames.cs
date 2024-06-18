using TranslateCS2.Core.Configurations;

namespace TranslateCS2.Core.Services.InstallPaths;
internal class PathDetectorXboxGames : APathDetector {
    public PathDetectorXboxGames() {
        this.InstallPath = AppConfigurationManager.BasicGamePassLocation;
    }
    public override bool Detect() {
        // TODO: AAA-2: would it be better to scan all folders within XboxGames for "Cities2.exe"?
        return base.Detect();
    }
}
