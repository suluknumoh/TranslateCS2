using System.Collections.Generic;
using System.Text;

using Colossal.Localization;

using Game.SceneFlow;

using TranslateCS2.Mod.Loggers;
using TranslateCS2.Mod.Models;
using TranslateCS2.ModBridge;

namespace TranslateCS2.Mod.Helpers;
internal static class ErrorMessageHelper {
    private static readonly LocalizationManager LocManager = GameManager.instance.localizationManager;
    public static void DisplayErrorMessage(IList<TranslationFile> erroneous, bool missing) {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine($"from {ModConstants.NameSimple} ({ModConstants.Name}):");
        builder.Append($"the following provided translationfiles are corrupt");
        if (missing) {
            builder.Append($" or got deleted");
        }
        builder.AppendLine();
        int counter = ModConstants.MaxErroneous;
        if (erroneous.Count > counter) {
            builder.AppendLine($"{erroneous.Count:N0} files are affected; only the first {counter:N0} are listed");
        }
        foreach (TranslationFile error in erroneous) {
            builder.AppendLine($"- {error.LocaleId}{ModConstants.JsonExtension} - {error.LocaleName}");
            --counter;
            if (counter <= 0) {
                break;
            }
        }
        if (missing) {
            builder.AppendLine($"you should be able to continue with the caveat that the previous loaded translations will be used");
        } else {
            builder.AppendLine($"you should be able to continue with the caveat that the affected translations will use the fallback-language '{LocManager.fallbackLocaleId}'");
        }
        SwitchAndDisplay(builder);
        Mod.Logger.LogError(typeof(ErrorMessageHelper), builder.ToString());
    }
    private static void SwitchAndDisplay(object obj) {
        Mod.Logger.SetShowsErrorsInUI(true);
        Mod.Logger.Error(obj);
        Mod.Logger.SetShowsErrorsInUI(false);
    }
}
