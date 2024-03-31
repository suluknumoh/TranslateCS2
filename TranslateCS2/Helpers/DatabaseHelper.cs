using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;

using TranslateCS2.Configurations;
using TranslateCS2.Databases;

namespace TranslateCS2.Helpers;
internal class DatabaseHelper {
    private static readonly string _sqliteExtension = $"{AppConfigurationManager.DatabaseExtension}";
    private static readonly string _searchPattern = $"*{_sqliteExtension}";
    private static readonly string _underscore = "_";
    private static readonly string _dateTimeFormatString = "yyyy-MM-dd_HH-mm-ss";
    private static readonly uint _backUpsToKeep = AppConfigurationManager.DatabaseMaxBackUpCount;
    public static void CreateIfNotExists() {
        string? assetPath = AppConfigurationManager.AssetPath;
        ArgumentNullException.ThrowIfNull(nameof(assetPath));
        if (!DatabaseExists()) {
            string databaseName = AppConfigurationManager.DatabaseName;
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

    /// <seealso href="https://stackoverflow.com/questions/71986869/disable-sqlite-automatic-database-creation-c-sharp"/>
    public static bool DatabaseExists() {
        return File.Exists(AppConfigurationManager.DatabaseName);
    }

    public static void BackUpIfExists(DatabaseBackUpIndicators backUpIndicator) {
        if (_backUpsToKeep == 0) {
            return;
        }
        string dateTimeString = DateTime.Now.ToString(_dateTimeFormatString);
        string? assetPath = AppConfigurationManager.AssetPath;
        ArgumentNullException.ThrowIfNull(nameof(assetPath));
        if (DatabaseExists()) {
            string backUpFileName = String.Concat(AppConfigurationManager.DatabaseNameRaw,
                                                    _underscore,
                                                    dateTimeString,
                                                    _underscore,
                                                    backUpIndicator.ToString(),
                                                    _sqliteExtension);
            string databaseName = AppConfigurationManager.DatabaseName;
            File.Copy(databaseName, backUpFileName);
            RemoveEldestBackUps(databaseName);
        }
    }

    private static void RemoveEldestBackUps(string databaseName) {
        string directory = Directory.GetCurrentDirectory();
        DirectoryInfo directoryInfo = new DirectoryInfo(directory);
        IEnumerable<FileInfo> backUps = directoryInfo.EnumerateFiles(_searchPattern).Where(f => f.Name != databaseName);
        IOrderedEnumerable<FileInfo> sqlitesOrdered = backUps.OrderByDescending(f => f.CreationTimeUtc);
        IEnumerable<FileInfo> deletes = sqlitesOrdered.Skip((int) _backUpsToKeep);
        foreach (FileInfo delete in deletes) {
            try {
                delete.Delete();
            } catch {
                // TODO: ???
            }
        }
    }
}
