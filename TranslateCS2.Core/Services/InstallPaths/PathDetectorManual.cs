using TranslateCS2.Core.Configurations.CitiesLocations;

namespace TranslateCS2.Core.Services.InstallPaths;
internal class PathDetectorManual : APathDetector {
    public PathDetectorManual() { }
    public override bool Detect() {
        try {
            CitiesLocationsSection? section = CitiesLocationsSection.GetReadOnly();
            if (section is not null) {
                foreach (CitiesLocation location in section.Locations) {
                    this.InstallPath = location.Path;
                    return base.Detect();
                }
            }
        } catch { }
        return false;
    }
}
