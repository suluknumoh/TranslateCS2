using System;
using System.IO;
using System.Xml;

using TranslateCS2.Inf;

namespace TranslateCS2.ZZZTestLib;
public class CS2PathsHelper {
    private static string TranslateCS2ModPropsPath { get; } = "..\\..\\..\\TranslateCS2.Inf\\Properties\\TranslateCS2.Mod.props";
    private static string EnvVariableNameManagedPath { get; } = "CSII_MANAGEDPATH";
    private static string EnvVariableNameToolPath { get; } = "CSII_TOOLPATH";
    private static string EnvVariableNameUserDataPath { get; } = "CSII_USERDATAPATH";
    private static string GameDll { get; } = "Game.dll";
    private static string ModProps { get; } = "Mod.props";


    public static string? GetStreamingDataPathFromProps() {
        string? managedPath = Environment.GetEnvironmentVariable(EnvVariableNameManagedPath, EnvironmentVariableTarget.User);
        if (managedPath != null
            && ExistsFileInPath(managedPath, GameDll)) {
            string managedPathJustified = JustifyPath(managedPath);
            return Paths.NormalizeUnix(managedPathJustified);
        }
        string? toolPath = Environment.GetEnvironmentVariable(EnvVariableNameToolPath, EnvironmentVariableTarget.User);
        if (toolPath != null
            && ExistsFileInPath(toolPath, ModProps)) {
            string modProps = Path.Combine(toolPath, ModProps);
            string? steamDefaultManagedPath = GetPropertyValueFromXml(modProps, ".//SteamDefaultManagedPath");
            if (steamDefaultManagedPath != null
                && ExistsFileInPath(steamDefaultManagedPath, GameDll)) {
                string steamDefaultManagedPathJustified = JustifyPath(steamDefaultManagedPath);
                return Paths.NormalizeUnix(steamDefaultManagedPathJustified);
            }
        }
        string? customManagedPath = GetPropertyValueFromXml(TranslateCS2ModPropsPath, ".//CustomManagedPath");
        string customManagedPathJustified = JustifyPath(customManagedPath);
        return Paths.NormalizeUnix(customManagedPathJustified);
    }

    private static string? GetPropertyValueFromXml(string path, string query) {
        XmlDocument xmlDoc = new XmlDocument();
        xmlDoc.Load(path);
        return xmlDoc.DocumentElement.SelectSingleNode(query).InnerText;
    }
    private static bool ExistsFileInPath(string path, string file) {
        string gameDll = Path.Combine(path, file);
        return File.Exists(gameDll);
    }
    public static string? GetUserDataPathFromEnvironment() {
        return Paths.NormalizeUnix(Environment.GetEnvironmentVariable(EnvVariableNameUserDataPath, EnvironmentVariableTarget.User));
    }
    private static string JustifyPath(string path) {
        return
            Path
                .Combine(path,
                         "..",
                         "StreamingAssets");
    }
}
