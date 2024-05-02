using System.IO;

using Colossal.PSI.Environment;

using TranslateCS2.Inf;

namespace TranslateCS2.Mod.Helpers;
/// <seealso cref="https://cs2.paradoxwikis.com/Naming_Folder_And_Files"/>
internal static class FileSystemHelper {
    // has to end with a forward-slash!
    public static string DataFolder { get; } = $"{EnvPath.kUserDataPath}/{ModConstants.ModsData}/{ModConstants.Name}/";
    public static void CreateIfNotExists() {
        Directory.CreateDirectory(DataFolder);
    }
}
