using System;
using System.IO;
using System.Linq;

using Microsoft.Win32;

using TranslateCS2.Inf;

namespace TranslateCS2.Core.Services.InstallPaths;
internal class PathDetectorSteam : APathDetector {
    private readonly uint appid = 949230;
    private readonly string registryKeyName = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Valve\\Steam";
    private readonly string registryValueName = "InstallPath";
    private readonly string steamappsFolderName = "steamapps";
    private readonly string commonFolderName = "common";

    public PathDetectorSteam() { }

    public override bool Detect() {
        try {
            string libraryFoldersVDF = this.GetLibraryFoldersVDFPath();
            string baseInstallPath = this.GetBaseInstallPath(libraryFoldersVDF);
            string installDir = this.GetInstallDirFromAppManifestACF(baseInstallPath);
            this.InstallPath = Path.Combine(baseInstallPath, this.steamappsFolderName, this.commonFolderName, installDir);
            return base.Detect();
        } catch {
            return false;
        }
    }

    private string GetInstallDirFromAppManifestACF(string baseInstallPath) {
        string installDirLine = $"{StringConstants.QuotationMark}installdir{StringConstants.QuotationMark}";
        string acf = Path.Combine(baseInstallPath, this.steamappsFolderName, $"appmanifest_{this.appid}.acf");
        string[] lines = File.ReadAllLines(acf);
        foreach (string line in lines) {
            if (line.Contains(installDirLine, StringComparison.OrdinalIgnoreCase)) {
                return EaseLine(line.Replace(installDirLine, String.Empty, StringComparison.OrdinalIgnoreCase));
            }
        }
        throw new ArgumentNullException();
    }

    private string GetBaseInstallPath(string libraryFoldersVDF) {
        string pathLine = $"{StringConstants.QuotationMark}Path{StringConstants.QuotationMark}";
        string[] lines = File.ReadAllLines(libraryFoldersVDF);
        bool start = false;
        foreach (string line in lines.Reverse()) {
            if (line.Contains($"{StringConstants.QuotationMark}{this.appid}{StringConstants.QuotationMark}")) {
                start = true;
            }
            if (start
                && line.Contains(pathLine, StringComparison.OrdinalIgnoreCase)) {
                return EaseLine(line.Replace(pathLine, String.Empty, StringComparison.OrdinalIgnoreCase));
            }
        }
        throw new ArgumentNullException();
    }

    private static string EaseLine(string v) {
        return v.Replace(StringConstants.Tab, String.Empty)
                .Replace(StringConstants.QuotationMark, String.Empty)
                .Replace(StringConstants.BackSlashDouble, StringConstants.BackSlash)
                .Trim();
    }

    private string GetLibraryFoldersVDFPath() {
        object? obj = Registry.GetValue(this.registryKeyName, this.registryValueName, null);
        if (obj is string installPath) {
            return Path.Combine(installPath, this.steamappsFolderName, "libraryfolders.vdf");
        }
        throw new ArgumentException();
    }
}
