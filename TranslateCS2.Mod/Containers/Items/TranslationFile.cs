using System.Collections.Generic;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Models.Localizations;

namespace TranslateCS2.Mod.Containers.Items;
public class TranslationFile : AMyLocalization<TranslationFileSource, IDictionary<string, string>, string> {
    private readonly string name;
    public override string Name { get; }
    public TranslationFile(string id,
                           string nameEnglish,
                           string name,
                           TranslationFileSource source) : base(id,
                                                                nameEnglish,
                                                                name,
                                                                source) {
        this.name = name;
        this.Name = this.GetName();
    }
    private string GetName() {
        if (this.Source.Localizations is not null) {
            if (this.Source.Localizations.TryGetValue(ModConstants.LocaleNameLocalizedKey, out string? outLocaleName)
                && outLocaleName is not null
                && !StringHelper.IsNullOrWhiteSpaceOrEmpty(outLocaleName)) {
                return outLocaleName;
            }
        }
        return this.name;
    }
}
