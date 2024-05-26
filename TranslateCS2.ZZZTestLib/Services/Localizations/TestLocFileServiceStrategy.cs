using TranslateCS2.Inf.Services.Localizations;

namespace TranslateCS2.ZZZTestLib.Services.Localizations;
public class TestLocFileServiceStrategy : LocFileServiceStrategy<string> {
    public override string CreateEntryValue(string key, string value) {
        return value;
    }
}
