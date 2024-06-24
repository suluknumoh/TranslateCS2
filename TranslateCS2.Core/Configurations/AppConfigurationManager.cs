using System;
using System.Configuration;

using TranslateCS2.Inf;

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
    public static string AppDialogMinHeight { get; } = "930";
    public static string AppDialogMinWidth { get; } = "1290";
    public static string AppMenuRegion { get; } = nameof(AppMenuRegion);
    public static string AppRibbonBarRegion { get; } = nameof(AppRibbonBarRegion);
    public static string AppMainRegion { get; } = nameof(AppMainRegion);
    public static string AppNewEditSessionRegion { get; } = nameof(AppNewEditSessionRegion);
    public static string AppExportImportRegion { get; } = nameof(AppExportImportRegion);
    public static string AppModuleDirectory { get; } = "./modules";
    /// <summary>
    ///     ends with a dot!
    /// </summary>
    public static string AssetPath { get; } = "TranslateCS2.Assets.";
    public static string LeadingLocFileName { get; } = $"en-US{ModConstants.LocExtension}";
    public static string LeadingLocLanguageCode { get; } = LeadingLocFileName.Split(".")[0];
    // INFO: dont translate! this value gets written into the database
    public static string NoneOverwrite { get; } = "none";
    public static string CheckLatestURL { get; } = "https://raw.githubusercontent.com/suluknumoh/TranslateCS2/main/latest";
    public static string ImExportDefaultFileName { get; } = $"translations{ModConstants.JsonExtension}";
    public static string ImExportFilter { get; } = $"JSON-File ({ModConstants.JsonExtension})|{ModConstants.JsonSearchPattern}";
    public static uint DatabaseMaxBackUpCount { get; private set; }
    public static string DatabaseExtension { get; } = ".sqlite";
    public static string DatabaseNameRaw { get; } = "Translations";
    public static string DatabaseName { get; } = $"{DatabaseNameRaw}{DatabaseExtension}";
    public static string DatabaseProvider { get; } = "SqliteConnection.SqliteConnectionFactory";
    public static bool SkipWorkAround { get; } = false;


    public static string ExeExtension { get; } = ".exe";
    public static string CitiesExe { get; } = $"Cities2{ExeExtension}";
    public static string Cities2_Data { get; } = nameof(Cities2_Data);
    public static TimeSpan MaxWaitOnRestart { get; } = TimeSpan.FromMinutes(1);



    public static string DatabaseConnectionString { get; } = $"Data Source=./{DatabaseName};Pooling=False";

    public static string DatabaseConnectionStringDebug { get; } = $"Data Source=../../../../../../{DatabaseName};Pooling=False";
}
