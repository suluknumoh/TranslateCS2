using System.Configuration;

namespace TranslateCS2.Configurations;
internal static class AppConfigurationManager {
    public static string? AppTitle => ConfigurationManager.AppSettings[nameof(AppTitle)];
    public static string? AppMinHeight => ConfigurationManager.AppSettings[nameof(AppMinHeight)];
    public static string? AppMinWidth => ConfigurationManager.AppSettings[nameof(AppMinWidth)];
    public static string? AppMenuRegion => ConfigurationManager.AppSettings[nameof(AppMenuRegion)];
    public static string? AppRibbonBarRegion => ConfigurationManager.AppSettings[nameof(AppRibbonBarRegion)];
    public static string? AppMainRegion => ConfigurationManager.AppSettings[nameof(AppMainRegion)];
    public static string? AppNewEditSessionRegion => ConfigurationManager.AppSettings[nameof(AppNewEditSessionRegion)];
    public static string? AssetPath => ConfigurationManager.AppSettings[nameof(AssetPath)];
    public static string? LeadingLocFileName => ConfigurationManager.AppSettings[nameof(LeadingLocFileName)];
}
