using TranslateCS2.Inf;
using TranslateCS2.Inf.Interfaces;

namespace TranslateCS2.ZZZTestLib.Services.Localizations;
public class TestLocFileDirectoryProvider : ILocFileDirectoryProvider {
    public string LocFileDirectory { get; } = CS2PathsHelper.GetStreamingDataPathFromProps() + StringConstants.BackSlash + StringConstants.DataTilde;
}
