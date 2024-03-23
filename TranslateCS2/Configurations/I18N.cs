using System.Reflection;
using System.Resources;

namespace TranslateCS2.Configurations;
internal static class I18N {
    // assetpath includes a dot at the end!
    private static readonly ResourceManager RM = new ResourceManager($"{AppConfigurationManager.AssetPath}i18n.i18n", Assembly.GetExecutingAssembly());

    public static string MessageAppUnusableWarning = RM.GetString(nameof(MessageAppUnusableWarning));
    public static string MessageConsistOfCharacters = RM.GetString(nameof(MessageConsistOfCharacters));
    public static string MessageLocalizationFileOverwriteDifferentMerge = RM.GetString(nameof(MessageLocalizationFileOverwriteDifferentMerge));
    public static string MessageLocalizationFileOverwriteOthersThan = RM.GetString(nameof(MessageLocalizationFileOverwriteOthersThan));
    public static string MessageUnderConstruction = RM.GetString(nameof(MessageUnderConstruction));
    public static string MessageNotEmptyOrWhitespace = RM.GetString(nameof(MessageNotEmptyOrWhitespace));
    public static string MessagePreparingTranslationExport = RM.GetString(nameof(MessagePreparingTranslationExport));
    public static string MessageSelectLocalizationFileOverwrite = RM.GetString(nameof(MessageSelectLocalizationFileOverwrite));
    public static string MessageTranslationReadyExport = RM.GetString(nameof(MessageTranslationReadyExport));
    public static string MessageTranslationSessionMergeFile = RM.GetString(nameof(MessageTranslationSessionMergeFile));
    public static string MessageTranslationSessionName = RM.GetString(nameof(MessageTranslationSessionName));
    public static string MessageTranslationSessionOverwriteFile = RM.GetString(nameof(MessageTranslationSessionOverwriteFile));
    public static string MessageTranslationSessionLocaleEnglish = RM.GetString(nameof(MessageTranslationSessionLocaleEnglish));
    public static string MessageTranslationSessionLocaleLocalized = RM.GetString(nameof(MessageTranslationSessionLocaleLocalized));


    public static string QuestionAreYouSure = RM.GetString(nameof(QuestionAreYouSure));


    public static string StringAutoDetectedInstallationDirectory = RM.GetString(nameof(StringAutoDetectedInstallationDirectory));
    public static string StringAutoDetectedLocFiles = RM.GetString(nameof(StringAutoDetectedLocFiles));
    public static string StringCreate = RM.GetString(nameof(StringCreate));
    public static string StringEdit = RM.GetString(nameof(StringEdit));
    public static string StringExport = RM.GetString(nameof(StringExport));
    public static string StringExported = RM.GetString(nameof(StringExported));
    public static string StringExporting = RM.GetString(nameof(StringExporting));
    public static string StringExportTranslation = RM.GetString(nameof(StringExportTranslation));
    public static string StringFilterKeys = RM.GetString(nameof(StringFilterKeys));
    public static string StringHideTranslated = RM.GetString(nameof(StringHideTranslated));
    public static string StringIn = RM.GetString(nameof(StringIn));
    public static string StringName = RM.GetString(nameof(StringName));
    public static string StringNew = RM.GetString(nameof(StringNew));
    public static string StringSearch = RM.GetString(nameof(StringSearch));
    public static string StringSearchFor = RM.GetString(nameof(StringSearchFor));
    public static string StringSelected = RM.GetString(nameof(StringSelected));
    public static string StringSession = RM.GetString(nameof(StringSession));
    public static string StringShowOnlyTranslated = RM.GetString(nameof(StringShowOnlyTranslated));
    public static string StringTranslationSessions = RM.GetString(nameof(StringTranslationSessions));
    public static string StringTranslationSessionMergeFile = RM.GetString(nameof(StringTranslationSessionMergeFile));
    public static string StringTranslationSessionOverwriteFile = RM.GetString(nameof(StringTranslationSessionOverwriteFile));
    public static string StringTranslationSessionLocaleEnglish = RM.GetString(nameof(StringTranslationSessionLocaleEnglish));
    public static string StringTranslationSessionLocaleLocalized = RM.GetString(nameof(StringTranslationSessionLocaleLocalized));


    public static string StringCountCap = RM.GetString(nameof(StringCountCap));
    public static string StringEditByOccurancesCap = RM.GetString(nameof(StringEditByOccurancesCap));
    public static string StringEditCap = RM.GetString(nameof(StringEditCap));
    public static string StringEnglishValueCap = RM.GetString(nameof(StringEnglishValueCap));
    public static string StringExportCap = RM.GetString(nameof(StringExportCap));
    public static string StringKeyCap = RM.GetString(nameof(StringKeyCap));
    public static string StringMergeValueCap = RM.GetString(nameof(StringMergeValueCap));
    public static string StringNavigationCap = RM.GetString(nameof(StringNavigationCap));
    public static string StringSessionManagementCap = RM.GetString(nameof(StringSessionManagementCap));
    public static string StringSessionsCap = RM.GetString(nameof(StringSessionsCap));
    public static string StringSettingsCap = RM.GetString(nameof(StringSettingsCap));
    public static string StringStartCap = RM.GetString(nameof(StringStartCap));
    public static string StringToolsCap = RM.GetString(nameof(StringToolsCap));
    public static string StringTranslationCap = RM.GetString(nameof(StringTranslationCap));
    public static string StringCreditsCaps = RM.GetString(nameof(StringCreditsCaps));


}
