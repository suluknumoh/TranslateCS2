using System.Collections.Generic;
using System.Text;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Loggers;

namespace TranslateCS2.Mod.Containers.Items;
public class ErrorMessages {
    private readonly IModRuntimeContainer runtimeContainer;
    internal ErrorMessages(IModRuntimeContainer runtimeContainer) {
        this.runtimeContainer = runtimeContainer;
    }
    private string Intro { get; } = $"from {ModConstants.NameSimple} ({ModConstants.Name}):";
    internal void DisplayErrorMessageForErroneous(IList<TranslationFile> erroneous, bool missing) {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(this.Intro);
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
            builder.AppendLine($"you should be able to continue with the caveat that the affected translations will use the fallback-language '{this.runtimeContainer?.LocManager?.fallbackLocaleId}'");
        }
        this.SwitchAndDisplay(builder);
        this.runtimeContainer.Logger?.LogError(typeof(ErrorMessages), builder.ToString());
    }
    public void DisplayErrorMessageFailedToGenerateJson() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(this.Intro);
        builder.AppendLine($"could not write");
        builder.AppendLine($"'{ModConstants.ModExportKeyValueJsonName}'");
        builder.AppendLine($"into");
        builder.AppendLine($"'{this.runtimeContainer.Paths.UserDataPath}'");
        builder.AppendLine("you should be able to continue");
        this.SwitchAndDisplay(builder);
    }
    private void SwitchAndDisplay(object obj) {
        this.runtimeContainer.Logger?.SetShowsErrorsInUI(true);
        this.runtimeContainer.Logger?.Error(obj);
        this.runtimeContainer.Logger?.SetShowsErrorsInUI(false);
    }
}
