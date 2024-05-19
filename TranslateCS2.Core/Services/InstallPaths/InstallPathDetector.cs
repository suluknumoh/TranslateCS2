using System;
using System.IO;
using System.Linq;

using Microsoft.Win32;

namespace TranslateCS2.Core.Services.InstallPaths;
internal class InstallPathDetector : IInstallPathDetector {
    private readonly uint appid = 949230;
    private readonly string registryKeyName = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Valve\\Steam";
    private readonly string registryValueName = "InstallPath";
    private readonly string steamappsFolderName = "steamapps";
    private readonly string commonFolderName = "common";
    public InstallPathDetector() { }

    public string DetectInstallPath() {
        string libraryFoldersVDF = this.GetLibraryFoldersVDFPath();
        string baseInstallPath = this.GetBaseInstallPath(libraryFoldersVDF);
        string installDir = this.GetInstallDirFromAppManifestACF(baseInstallPath);
        return Path.Combine(baseInstallPath, this.steamappsFolderName, this.commonFolderName, installDir);
    }

    private string GetInstallDirFromAppManifestACF(string baseInstallPath) {
        string acf = Path.Combine(baseInstallPath, this.steamappsFolderName, $"appmanifest_{this.appid}.acf");
        string[] lines = File.ReadAllLines(acf);
        foreach (string line in lines) {
            if (line.Contains("\"installdir\"", StringComparison.OrdinalIgnoreCase)) {
                return EaseLine(line.Replace("\"installdir\"", String.Empty, StringComparison.OrdinalIgnoreCase));
            }
        }
        throw new ArgumentNullException();
    }

    private string GetBaseInstallPath(string libraryFoldersVDF) {
        string[] lines = File.ReadAllLines(libraryFoldersVDF);
        bool start = false;
        foreach (string line in lines.Reverse()) {
            if (line.Contains($"\"{this.appid}\"")) {
                start = true;
            }
            if (start && line.Contains("\"Path\"", StringComparison.OrdinalIgnoreCase)) {
                return EaseLine(line.Replace("\"Path\"", String.Empty, StringComparison.OrdinalIgnoreCase));
            }
        }
        throw new ArgumentNullException();
    }

    private static string EaseLine(string v) {
        return v.Replace("\t", String.Empty)
                .Replace("\"", String.Empty)
                .Replace(@"\\", @"\")
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
