using System;

using Colossal.Localization;

using Game.SceneFlow;

namespace TranslateCS2.Mod.Helpers;
internal static class LocaleNameHelper {
    private static readonly LocalizationManager LocalizationManager = GameManager.instance.localizationManager;
    public static bool IsLocaleNameAvailable(string localeName) {
        string[] alreadySupportedLocales = LocalizationManager.GetSupportedLocales();
        foreach (string alreadySupportedLocale in alreadySupportedLocales) {
            string alreadySupportedLocaleName = LocalizationManager.GetLocalizedName(alreadySupportedLocale);
            if (localeName.Equals(alreadySupportedLocaleName, StringComparison.OrdinalIgnoreCase)) {
                return false;
            }
        }
        return true;
    }
}
