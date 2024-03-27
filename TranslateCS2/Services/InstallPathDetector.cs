using System;
using System.IO;
using System.Linq;

using Microsoft.Win32;

namespace TranslateCS2.Services;
internal class InstallPathDetector {
    private readonly uint _appid = 949230;
    private readonly string _registryKeyName = "HKEY_LOCAL_MACHINE\\SOFTWARE\\Wow6432Node\\Valve\\Steam";
    private readonly string _registryValueName = "InstallPath";
    private readonly string _steamappsFolderName = "steamapps";
    private readonly string _commonFolderName = "common";
    public InstallPathDetector() { }

    public string DetectInstallPath() {
        string libraryFoldersVDF = this.GetLibraryFoldersVDFPath();
        string baseInstallPath = this.GetBaseInstallPath(libraryFoldersVDF);
        string installDir = this.GetInstallDirFromAppManifestACF(baseInstallPath);
        return Path.Combine(baseInstallPath, this._steamappsFolderName, this._commonFolderName, installDir);
    }

    private string GetInstallDirFromAppManifestACF(string baseInstallPath) {
        string acf = Path.Combine(baseInstallPath, this._steamappsFolderName, $"appmanifest_{this._appid}.acf");
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
            if (line.Contains($"\"{this._appid}\"")) {
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
        object? obj = Registry.GetValue(this._registryKeyName, this._registryValueName, null);
        if (obj is string installPath) {
            return Path.Combine(installPath, this._steamappsFolderName, "libraryfolders.vdf");
        }
        throw new ArgumentException();
    }
}
