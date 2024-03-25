using System;
using System.Reflection;
using System.Resources;

namespace TranslateCS2.Configurations;
internal static class I18N {
    // assetpath includes a dot at the end!
    private static readonly ResourceManager RM = new ResourceManager($"{AppConfigurationManager.AssetPath}i18n.i18n", Assembly.GetExecutingAssembly());

    public static string MessageAppUnusableWarning { get; } = RM.GetString(nameof(MessageAppUnusableWarning)) ?? String.Empty;
    public static string MessageConsistOfCharacters { get; } = RM.GetString(nameof(MessageConsistOfCharacters)) ?? String.Empty;
    public static string MessageLocalizationFileOverwriteDifferentMerge { get; } = RM.GetString(nameof(MessageLocalizationFileOverwriteDifferentMerge)) ?? String.Empty;
    public static string MessageLocalizationFileOverwriteOthersThan { get; } = RM.GetString(nameof(MessageLocalizationFileOverwriteOthersThan)) ?? String.Empty;
    public static string MessageUnderConstruction { get; } = RM.GetString(nameof(MessageUnderConstruction)) ?? String.Empty;
    public static string MessageNotEmptyOrWhitespace { get; } = RM.GetString(nameof(MessageNotEmptyOrWhitespace)) ?? String.Empty;
    public static string MessagePreparingTranslationExport { get; } = RM.GetString(nameof(MessagePreparingTranslationExport)) ?? String.Empty;
    public static string MessageSelectLocalizationFileOverwrite { get; } = RM.GetString(nameof(MessageSelectLocalizationFileOverwrite)) ?? String.Empty;
    public static string MessageTranslationReadyExport { get; } = RM.GetString(nameof(MessageTranslationReadyExport)) ?? String.Empty;
    public static string MessageTranslationSessionMergeFile { get; } = RM.GetString(nameof(MessageTranslationSessionMergeFile)) ?? String.Empty;
    public static string MessageTranslationSessionName { get; } = RM.GetString(nameof(MessageTranslationSessionName)) ?? String.Empty;
    public static string MessageTranslationSessionOverwriteFile { get; } = RM.GetString(nameof(MessageTranslationSessionOverwriteFile)) ?? String.Empty;
    public static string MessageTranslationSessionLocaleEnglish { get; } = RM.GetString(nameof(MessageTranslationSessionLocaleEnglish)) ?? String.Empty;
    public static string MessageTranslationSessionLocaleLocalized { get; } = RM.GetString(nameof(MessageTranslationSessionLocaleLocalized)) ?? String.Empty;


    public static string QuestionAreYouSure { get; } = RM.GetString(nameof(QuestionAreYouSure)) ?? String.Empty;


    public static string StringAutoDetectedInstallationDirectory { get; } = RM.GetString(nameof(StringAutoDetectedInstallationDirectory)) ?? String.Empty;
    public static string StringAutoDetectedLocFiles { get; } = RM.GetString(nameof(StringAutoDetectedLocFiles)) ?? String.Empty;
    public static string StringCreate { get; } = RM.GetString(nameof(StringCreate)) ?? String.Empty;
    public static string StringEdit { get; } = RM.GetString(nameof(StringEdit)) ?? String.Empty;
    public static string StringExport { get; } = RM.GetString(nameof(StringExport)) ?? String.Empty;
    public static string StringExported { get; } = RM.GetString(nameof(StringExported)) ?? String.Empty;
    public static string StringExporting { get; } = RM.GetString(nameof(StringExporting)) ?? String.Empty;
    public static string StringExportTranslation { get; } = RM.GetString(nameof(StringExportTranslation)) ?? String.Empty;
    public static string StringFilterKeys { get; } = RM.GetString(nameof(StringFilterKeys)) ?? String.Empty;
    public static string StringHideTranslated { get; } = RM.GetString(nameof(StringHideTranslated)) ?? String.Empty;
    public static string StringIn { get; } = RM.GetString(nameof(StringIn)) ?? String.Empty;
    public static string StringName { get; } = RM.GetString(nameof(StringName)) ?? String.Empty;
    public static string StringNew { get; } = RM.GetString(nameof(StringNew)) ?? String.Empty;
    public static string StringSearch { get; } = RM.GetString(nameof(StringSearch)) ?? String.Empty;
    public static string StringSearchFor { get; } = RM.GetString(nameof(StringSearchFor)) ?? String.Empty;
    public static string StringSelected { get; } = RM.GetString(nameof(StringSelected)) ?? String.Empty;
    public static string StringSession { get; } = RM.GetString(nameof(StringSession)) ?? String.Empty;
    public static string StringShowOnlyTranslated { get; } = RM.GetString(nameof(StringShowOnlyTranslated)) ?? String.Empty;
    public static string StringTranslationSessions { get; } = RM.GetString(nameof(StringTranslationSessions)) ?? String.Empty;
    public static string StringTranslationSessionMergeFile { get; } = RM.GetString(nameof(StringTranslationSessionMergeFile)) ?? String.Empty;
    public static string StringTranslationSessionOverwriteFile { get; } = RM.GetString(nameof(StringTranslationSessionOverwriteFile)) ?? String.Empty;
    public static string StringTranslationSessionLocaleEnglish { get; } = RM.GetString(nameof(StringTranslationSessionLocaleEnglish)) ?? String.Empty;
    public static string StringTranslationSessionLocaleLocalized { get; } = RM.GetString(nameof(StringTranslationSessionLocaleLocalized)) ?? String.Empty;


    public static string StringCountCap { get; } = RM.GetString(nameof(StringCountCap)) ?? String.Empty;
    public static string StringEditByOccurancesCap { get; } = RM.GetString(nameof(StringEditByOccurancesCap)) ?? String.Empty;
    public static string StringEditCap { get; } = RM.GetString(nameof(StringEditCap)) ?? String.Empty;
    public static string StringEnglishValueCap { get; } = RM.GetString(nameof(StringEnglishValueCap)) ?? String.Empty;
    public static string StringExportCap { get; } = RM.GetString(nameof(StringExportCap)) ?? String.Empty;
    public static string StringKeyCap { get; } = RM.GetString(nameof(StringKeyCap)) ?? String.Empty;
    public static string StringMergeValueCap { get; } = RM.GetString(nameof(StringMergeValueCap)) ?? String.Empty;
    public static string StringNavigationCap { get; } = RM.GetString(nameof(StringNavigationCap)) ?? String.Empty;
    public static string StringSessionManagementCap { get; } = RM.GetString(nameof(StringSessionManagementCap)) ?? String.Empty;
    public static string StringSessionsCap { get; } = RM.GetString(nameof(StringSessionsCap)) ?? String.Empty;
    public static string StringSettingsCap { get; } = RM.GetString(nameof(StringSettingsCap)) ?? String.Empty;
    public static string StringStartCap { get; } = RM.GetString(nameof(StringStartCap)) ?? String.Empty;
    public static string StringToolsCap { get; } = RM.GetString(nameof(StringToolsCap)) ?? String.Empty;
    public static string StringTranslationCap { get; } = RM.GetString(nameof(StringTranslationCap)) ?? String.Empty;
    public static string StringCreditsCaps { get; } = RM.GetString(nameof(StringCreditsCaps)) ?? String.Empty;


}
