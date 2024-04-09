﻿using System;
using System.Configuration;

namespace TranslateCS2.Core.Configurations;
public static class AppConfigurationManager {

    static AppConfigurationManager() {
        InitDatabaseMaxUpdateCount();
    }

    private static void InitDatabaseMaxUpdateCount() {
        string? s = ConfigurationManager.AppSettings[nameof(DatabaseMaxBackUpCount)];
        bool parsed = UInt32.TryParse(s, out uint result);
        if (parsed) {
            DatabaseMaxBackUpCount = result;
        } else {
            DatabaseMaxBackUpCount = 20;
        }
    }

    public static string AppTitle { get; } = "TranslateCS2 - an unofficial Translator-Tool for Cities: Skylines 2";
    public static string AppMinHeight { get; } = "800";
    public static string AppMinWidth { get; } = "1260";
    public static string AppDialogMinHeight { get; } = "760";
    public static string AppDialogMinWidth { get; } = AppMinWidth;
    public static string AppMenuRegion { get; } = nameof(AppMenuRegion);
    public static string AppRibbonBarRegion { get; } = nameof(AppRibbonBarRegion);
    public static string AppMainRegion { get; } = nameof(AppMainRegion);
    public static string AppNewEditSessionRegion { get; } = nameof(AppNewEditSessionRegion);
    public static string AppExportImportRegion { get; } = nameof(AppExportImportRegion);
    public static string AppSelectedSessionInfoRegionImport { get; } = nameof(AppSelectedSessionInfoRegionImport);
    public static string AppSelectedSessionInfoRegionExport { get; } = nameof(AppSelectedSessionInfoRegionExport);
    public static string AppModuleDirectory { get; } = "./modules";
    /// <summary>
    ///     ends with a dot!
    /// </summary>
    public static string AssetPath { get; } = "TranslateCS2.Assets.";
    public static string LeadingLocFileName { get; } = "en-US.loc";
    public static string LeadingLocLanguageCode { get; } = LeadingLocFileName.Split(".")[0];
    public static string CheckLatestURL { get; } = "https://raw.githubusercontent.com/suluknumoh/TranslateCS2/main/latest";
    public static string ImExportDefaultFileName { get; } = "translations.json";
    public static string ImExportFilter { get; } = "JSON-File (*.json)|*.json";
    public static string ImExportFileExtension { get; } = ".json";
    public static uint DatabaseMaxBackUpCount { get; private set; }
    public static string DatabaseExtension { get; } = ".sqlite";
    public static string DatabaseNameRaw { get; } = "Translations";
    public static string DatabaseName { get; } = $"{DatabaseNameRaw}{DatabaseExtension}";
    public static string DatabaseProvider { get; } = "SqliteConnection.SqliteConnectionFactory";
    public static bool SkipWorkAround { get; } = false;



    public static string DatabaseConnectionString { get; } = $"Data Source=./{DatabaseName};Pooling=False";

    public static string DatabaseConnectionStringDebug { get; } = $"Data Source=../../../../../../{DatabaseName};Pooling=False";

}