using TranslateCS2.Core.Configurations.CitiesLocations;

namespace TranslateCS2.Core.Services.InstallPaths;
internal class PathDetectorManual : APathDetector {
    public PathDetectorManual() { }
    public override bool Detect() {
        try {
            CitiesLocationsSection? section = CitiesLocationsSection.GetReadOnly();
            if (section is not null) {
                foreach (CitiesLocation location in section.Locations) {
                    // only one location is supported
                    /// <see cref="TranslateCS2.Core.Configurations.CitiesLocations.CitiesLocationsSection.AddLocation(System.String)"/>
                    // but the foreach-way 'checks' if Locations is empty and uses the first entry
                    this.InstallPath = location.Path;
                    return base.Detect();
                }
            }
        } catch { }
        return false;
    }
}
