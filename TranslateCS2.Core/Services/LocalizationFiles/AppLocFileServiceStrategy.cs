using TranslateCS2.Core.Models.Localizations;
using TranslateCS2.Inf.Services.Localizations;

namespace TranslateCS2.Core.Services.LocalizationFiles;
internal class AppLocFileServiceStrategy : LocFileServiceStrategy<IAppLocFileEntry> {
    public override IAppLocFileEntry CreateEntryValue(string key, string value) {
        return AppLocFileEntryFactory.Create(key, value);
    }
}
