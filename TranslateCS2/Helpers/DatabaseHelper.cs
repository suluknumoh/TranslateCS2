using System;
using System.Configuration;
using System.IO;
using System.Reflection;

using TranslateCS2.Configurations;

namespace TranslateCS2.Helpers;
internal class DatabaseHelper {
    internal static void CreateIfNotExists() {
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
}
