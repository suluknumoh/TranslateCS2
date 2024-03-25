using System;
using System.Configuration;

namespace TranslateCS2.Configurations;
internal static class AppConfigurationManager {
    public static string AppTitle { get; } = ConfigurationManager.AppSettings[nameof(AppTitle)] ?? String.Empty;
    public static string AppMinHeight { get; } = ConfigurationManager.AppSettings[nameof(AppMinHeight)] ?? String.Empty;
    public static string AppMinWidth { get; } = ConfigurationManager.AppSettings[nameof(AppMinWidth)] ?? String.Empty;
    public static string AppMenuRegion { get; } = ConfigurationManager.AppSettings[nameof(AppMenuRegion)] ?? String.Empty;
    public static string AppRibbonBarRegion { get; } = ConfigurationManager.AppSettings[nameof(AppRibbonBarRegion)] ?? String.Empty;
    public static string AppMainRegion { get; } = ConfigurationManager.AppSettings[nameof(AppMainRegion)] ?? String.Empty;
    public static string AppNewEditSessionRegion { get; } = ConfigurationManager.AppSettings[nameof(AppNewEditSessionRegion)] ?? String.Empty;
    /// <summary>
    ///     ends with a dot!
    /// </summary>
    public static string AssetPath { get; } = ConfigurationManager.AppSettings[nameof(AssetPath)] ?? String.Empty;
    public static string LeadingLocFileName { get; } = ConfigurationManager.AppSettings[nameof(LeadingLocFileName)] ?? String.Empty;
    public static string CheckLatestURL { get; } = ConfigurationManager.AppSettings[nameof(CheckLatestURL)] ?? String.Empty;
}
