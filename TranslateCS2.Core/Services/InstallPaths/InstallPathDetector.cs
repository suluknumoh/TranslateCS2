using System.Collections.Generic;
using System.IO;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Interfaces;

namespace TranslateCS2.Core.Services.InstallPaths;
internal class InstallPathDetector : APathDetector, ILocFileDirectoryProvider {
    private readonly string cities2DataFolderName = "Cities2_Data";
    private readonly string streamingAssetsFolderName = "StreamingAssets";
    private bool isDetected;
    private readonly IList<IInstallPathDetector> pathDetectors = [];


    public string LocFileDirectory => Path.Combine(this.InstallPath,
                                                   this.cities2DataFolderName,
                                                   this.streamingAssetsFolderName,
                                                   StringConstants.DataTilde);


    public InstallPathDetector() {
        this.pathDetectors.Add(new PathDetectorSteam());
        this.pathDetectors.Add(new PathDetectorXboxGames());
        this.pathDetectors.Add(new PathDetectorManual());

    }
    public override bool Detect() {
        if (this.isDetected) {
            return this.isDetected;
        }
        foreach (IInstallPathDetector detector in this.pathDetectors) {
            this.isDetected = detector.Detect();
            if (this.isDetected) {
                this.InstallPath = detector.InstallPath;
                break;
            }
        }
        return this.isDetected;
    }
}
