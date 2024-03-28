using System;
using System.Configuration;
using System.IO;
using System.Reflection;

using TranslateCS2.Configurations;

namespace TranslateCS2.Helpers;
internal class DatabaseHelper {
    public static void CreateIfNotExists() {
        string? assetPath = AppConfigurationManager.AssetPath;
        ArgumentNullException.ThrowIfNull(nameof(assetPath));
        foreach (ConnectionStringSettings item in ConfigurationManager.ConnectionStrings) {
            string databaseName = item.Name;
            if (databaseName.EndsWith(".sqlite") && !File.Exists(databaseName)) {
                Assembly assembly = Assembly.GetExecutingAssembly();
                using Stream? databaseAssetStream = assembly.GetManifestResourceStream(assetPath + databaseName);
                ArgumentNullException.ThrowIfNull(databaseAssetStream, nameof(databaseAssetStream));
                using Stream output = File.OpenWrite(databaseName);
                byte[] buffer = new byte[1024];
                while (databaseAssetStream.Read(buffer) > 0) {
                    output.Write(buffer);
                }
            }
        }
    }

    /// <seealso href="https://stackoverflow.com/questions/71986869/disable-sqlite-automatic-database-creation-c-sharp"/>
    public static bool DatabaseExists(ConnectionStringSettings connectionStringSettings) {
        string? assetPath = AppConfigurationManager.AssetPath;
        ArgumentNullException.ThrowIfNull(nameof(assetPath));
        string databaseName = connectionStringSettings.Name;
        if (databaseName.EndsWith(".sqlite")) {
            return File.Exists(databaseName);
        }
        return false;
    }
}
