using System;
using System.IO;
using System.Xml;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Containers;

namespace TranslateCS2.ZModDevHelper;
internal class ModTestRuntimeContainer : AModRuntimeContainer {
    private static string TranslateCS2ModPropsPath { get; } = "..\\..\\..\\TranslateCS2.Inf\\Properties\\TranslateCS2.Mod.props";
    private static string EnvVariableNameManagedPath { get; } = "CSII_MANAGEDPATH";
    private static string EnvVariableNameToolPath { get; } = "CSII_TOOLPATH";
    private static string GameDll { get; } = "Game.dll";
    private static string ModProps { get; } = "Mod.props";


    public ModTestRuntimeContainer() : base(true, new Paths(true,
                                                                             GetStreamingDataPathFromProps())) { }


    private static string GetStreamingDataPathFromProps() {
        string managedPath = Environment.GetEnvironmentVariable(EnvVariableNameManagedPath, EnvironmentVariableTarget.User);
        if (ExistsFileInPath(managedPath, GameDll)) {
            return JustifyPath(managedPath);
        }
        string toolPath = Environment.GetEnvironmentVariable(EnvVariableNameToolPath, EnvironmentVariableTarget.User);
        if (ExistsFileInPath(toolPath, ModProps)) {
            string modProps = Path.Combine(toolPath, ModProps);
            string steamDefaultManagedPath = GetPropertyValueFromXml(modProps, ".//SteamDefaultManagedPath");
            if (ExistsFileInPath(steamDefaultManagedPath, GameDll)) {
                return JustifyPath(steamDefaultManagedPath);
            }
        }
        string customManagedPath = GetPropertyValueFromXml(TranslateCS2ModPropsPath, ".//CustomManagedPath");
        return JustifyPath(customManagedPath);
    }

    private static string GetPropertyValueFromXml(string path, string query) {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);
        return xmlDoc.DocumentElement.SelectSingleNode(query).InnerText;
    }
    private static bool ExistsFileInPath(string path, string file) {
        string gameDll = Path.Combine(path, file);
        return File.Exists(gameDll);
    }
    public static string JustifyPath(string path) {
        return
            Path
                .Combine(path,
                         "..",
                         "StreamingAssets")
                .Replace("\\", "/");
    }
}
