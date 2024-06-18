using TranslateCS2.Inf;
using TranslateCS2.Inf.Services.Localizations;

namespace TranslateCS2.ZZZTestLib.Services.Localizations;
public class TestLocFileServiceStrategy : LocFileServiceStrategyBuiltIn<string> {
    public override string LocFileDirectory { get; } = CS2PathsHelper.GetStreamingDataPathFromProps() + StringConstants.BackSlash + StringConstants.DataTilde;
    protected override string CreateEntryValue(string key, string value) {
        return value;
    }
}
