using System.Collections.Generic;
using System.Linq;
using System.Text;

using TranslateCS2.Inf;

namespace TranslateCS2.Mod.Containers.Items;
internal class ErrorMessages {
    private readonly IModRuntimeContainer runtimeContainer;
    public ErrorMessages(IModRuntimeContainer runtimeContainer) {
        this.runtimeContainer = runtimeContainer;
    }
    public static string Intro { get; } = $"from {ModConstants.NameSimple} ({ModConstants.Name}):";
    public void DisplayErrorMessageForErroneous(IEnumerable<FlavorSource> erroneous, bool missing) {
        // TODO: differentiate by FlavorSourceTypes
        // TODO: also display FlavorSource.ModName
        // TODO: also display FlavorSource.ModId?
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(Intro);
        builder.Append($"the following provided translationfiles are corrupt");
        if (missing) {
            builder.Append($" or got deleted");
        }
        builder.AppendLine();
        ListErroneous(erroneous, builder);
        if (missing) {
            builder.AppendLine($"you should be able to continue with the caveat that the previous loaded translations will be used");
        } else {
            builder.AppendLine($"you should be able to continue with the caveat that the affected translations will use the fallback-language '{this.runtimeContainer.LocManager.FallbackLocaleId}'");
        }
        this.runtimeContainer.Logger.DisplayError(builder);
        this.runtimeContainer.Logger.LogError(typeof(ErrorMessages), builder.ToString());
    }

    private static void ListErroneous(IEnumerable<FlavorSource> erroneous, StringBuilder builder) {
        int counter = ModConstants.MaxErroneous;
        if (erroneous.Count() > counter) {
            builder.AppendLine($"{erroneous.Count():N0} files are affected; only the first {counter:N0} are listed");
        }
        foreach (FlavorSource error in erroneous) {
            builder.AppendLine($"- {error.Localization.Id}{ModConstants.JsonExtension} - {error.Localization.NameEnglish}");
            --counter;
            if (counter <= 0) {
                break;
            }
        }
    }

    public void DisplayErrorMessageFailedToGenerateJson() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(Intro);
        builder.AppendLine($"could not write");
        builder.AppendLine($"'{ModConstants.ModExportKeyValueJsonName}'");
        builder.AppendLine($"into");
        builder.AppendLine($"'{this.runtimeContainer.Paths.ModsDataPathSpecific}'");
        builder.AppendLine("you should be able to continue");
        this.runtimeContainer.Logger.DisplayError(builder);
        this.runtimeContainer.Logger.LogError(typeof(ErrorMessages), builder.ToString());
    }
}
