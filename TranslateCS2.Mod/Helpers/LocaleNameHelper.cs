using System;

using Game.SceneFlow;

namespace TranslateCS2.Mod.Helpers;
internal static class LocaleNameHelper {
    public static bool IsLocaleNameAvailable(string localeName) {
        string[] alreadySupportedLocales = GameManager.instance.localizationManager.GetSupportedLocales();
        foreach (string alreadySupportedLocale in alreadySupportedLocales) {
            string alreadySupportedLocaleName = GameManager.instance.localizationManager.GetLocalizedName(alreadySupportedLocale);
            if (localeName.Equals(alreadySupportedLocaleName, StringComparison.OrdinalIgnoreCase)) {
                return false;
            }
        }
        return true;
    }
}
