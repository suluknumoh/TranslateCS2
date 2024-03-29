using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.Reflection;

using TranslateCS2.Configurations;
using TranslateCS2.Databases;

namespace TranslateCS2.Helpers;
internal class DatabaseHelper {
    private static readonly string _sqliteExtension = ".sqlite";
    private static readonly string _searchPattern = $"*{_sqliteExtension}";
    private static readonly string _underscore = "_";
    private static readonly string _dateTimeFormatString = "yyyy-MM-dd_HH-mm-ss";
    private static readonly int _backUpsToKeep = 20;
    public static void CreateIfNotExists() {
        string? assetPath = AppConfigurationManager.AssetPath;
        ArgumentNullException.ThrowIfNull(nameof(assetPath));
        foreach (ConnectionStringSettings item in ConfigurationManager.ConnectionStrings) {
            string databaseName = item.Name;
            if (databaseName.EndsWith(_sqliteExtension) && !File.Exists(databaseName)) {
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
        if (databaseName.EndsWith(_sqliteExtension)) {
            return File.Exists(databaseName);
        }
        return false;
    }

    public static void BackUpIfExists(DatabaseBackUpIndicators backUpIndicator) {
        string dateTimeString = DateTime.Now.ToString(_dateTimeFormatString);
        string? assetPath = AppConfigurationManager.AssetPath;
        ArgumentNullException.ThrowIfNull(nameof(assetPath));
        foreach (ConnectionStringSettings item in ConfigurationManager.ConnectionStrings) {
            string databaseName = item.Name;
            if (databaseName.EndsWith(_sqliteExtension) && File.Exists(databaseName)) {
                RemoveEldestBackUp(databaseName);
                string splitted = databaseName.Split(_sqliteExtension)[0];
                string backUpFileName = String.Concat(splitted,
                                                      _underscore,
                                                      dateTimeString,
                                                      _underscore,
                                                      backUpIndicator.ToString(),
                                                      _sqliteExtension);
                File.Copy(databaseName, backUpFileName);
            }
        }
    }

    private static void RemoveEldestBackUp(string databaseName) {
        string directory = Directory.GetCurrentDirectory();
        DirectoryInfo directoryInfo = new DirectoryInfo(directory);
        IEnumerable<FileInfo> backUps = directoryInfo.EnumerateFiles(_searchPattern).Where(f => f.Name != databaseName);
        IOrderedEnumerable<FileInfo> sqlitesOrdered = backUps.OrderByDescending(f => f.CreationTimeUtc);
        IEnumerable<FileInfo> deletes = sqlitesOrdered.Skip(_backUpsToKeep - 1);
        foreach (FileInfo delete in deletes) {
            try {
                delete.Delete();
            } catch {
                // TODO: ???
            }
        }
    }
}
