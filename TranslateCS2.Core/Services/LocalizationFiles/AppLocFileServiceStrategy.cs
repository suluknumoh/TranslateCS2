using TranslateCS2.Core.Models.Localizations;
using TranslateCS2.Inf.Interfaces;
using TranslateCS2.Inf.Services.Localizations;

namespace TranslateCS2.Core.Services.LocalizationFiles;
internal class AppLocFileServiceStrategy : LocFileServiceStrategyBuiltIn<IAppLocFileEntry> {
    private readonly ILocFileDirectoryProvider locFileDirectoryProvider;
    public override string LocFileDirectory => this.locFileDirectoryProvider.LocFileDirectory;
    public AppLocFileServiceStrategy(ILocFileDirectoryProvider locFileDirectoryProvider) {
        this.locFileDirectoryProvider = locFileDirectoryProvider;
    }
    protected override IAppLocFileEntry CreateEntryValue(string key, string value) {
        return AppLocFileEntryFactory.Create(key, value);
    }
}
