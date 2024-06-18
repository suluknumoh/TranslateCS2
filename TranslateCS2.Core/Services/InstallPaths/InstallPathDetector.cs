using System.Collections.Generic;
using System.IO;

using TranslateCS2.Core.Helpers;
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
        if (!this.isDetected) {
            bool ok = false;
            // TODO: AAA-7: show an info
            // TODO: AAA-8: show open file dialog
            // TODO: AAA-9: double-check
            // TODO: AAA-10: write selected path to
            /// <see cref="TranslateCS2.Core.Configurations.AppConfigurationManager.CitiesLocation"/>
            // TODO: AAA-11: and restart
            if (ok) {
                bool restarted = RestartHelper.Restart();
                if (!restarted) {
                    // TODO: AAA-12: show an info
                }
            }
        }
        return this.isDetected;
    }
}
