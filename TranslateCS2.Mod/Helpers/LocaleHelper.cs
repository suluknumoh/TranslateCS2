using System;
using System.Collections.Generic;
using System.Linq;

using Colossal.Localization;

using Game.SceneFlow;

namespace TranslateCS2.Mod.Helpers;
internal static class LocaleHelper {
    private static readonly LocalizationManager LocalizationManager = GameManager.instance.localizationManager;
    public static IList<string> BuiltIn { get; } = [
        "de-DE",
        "en-US",
        "es-ES",
        "fr-FR",
        "it-IT",
        "ja-JP",
        "ko-KR",
        "pl-PL",
        "pt-BR",
        "ru-RU",
        "zh-HANS",
        "zh_HANT"
    ];
    public static bool MapsToExisting(string localeId) {
        HashSet<string> set = LocalizationManager.GetSupportedLocales().Select(i => i.Split('-')[0]).ToHashSet();
        return set.Contains(localeId.Split('-')[0]);
    }

    public static string GetExisting(string localeId) {
        string id = $"{localeId.Split('-')[0]}-";
        IEnumerable<string> list = LocalizationManager.GetSupportedLocales().Where(item => item.StartsWith(id, StringComparison.OrdinalIgnoreCase));
        if (!list.Any()) {
            return localeId;
        }
        return list.First();
    }
}
