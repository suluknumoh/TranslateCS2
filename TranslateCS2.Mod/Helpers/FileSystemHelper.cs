using System.IO;

using Colossal.PSI.Environment;

using TranslateCS2.ModBridge;

namespace TranslateCS2.Mod.Helpers;
/// <seealso href="https://cs2.paradoxwikis.com/Naming_Folder_And_Files"/>
internal static class FileSystemHelper {
    // has to end with a forward-slash!
    public static string DataFolder { get; } = $"{EnvPath.kUserDataPath}/{ModConstants.ModsData}/{ModConstants.Name}/";
    public static string SettingsFolder { get; } = $"{EnvPath.kUserDataPath}/{ModConstants.ModsSettings}/{ModConstants.Name}";
    static FileSystemHelper() {
        Directory.CreateDirectory(DataFolder);
        Directory.CreateDirectory(SettingsFolder);
    }
}
