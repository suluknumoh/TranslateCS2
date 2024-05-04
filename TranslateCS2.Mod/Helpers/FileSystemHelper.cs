using System.IO;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers;

namespace TranslateCS2.Mod.Helpers;
/// <seealso cref="https://cs2.paradoxwikis.com/Naming_Folder_And_Files"/>
public class FileSystemHelper {
    private readonly IModRuntimeContainer runtimeContainer;
    // has to end with a forward-slash!
    public string DataFolder => $"{this.runtimeContainer.UserDataPath}/{ModConstants.ModsData}/{ModConstants.Name}/";
    public FileSystemHelper(IModRuntimeContainer runtimeContainer) {
        this.runtimeContainer = runtimeContainer;
        this.CreateIfNotExists();
    }
    private void CreateIfNotExists() {
        Directory.CreateDirectory(this.DataFolder);
    }
}
