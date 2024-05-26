using TranslateCS2.Inf;
using TranslateCS2.Inf.Interfaces;

namespace TranslateCS2.ZZZTestLib.Services.Localizations;
public class TestStreamingDatasDataPathProvider : IStreamingDatasDataPathProvider {
    public string StreamingDatasDataPath { get; } = CS2PathsHelper.GetStreamingDataPathFromProps() + StringConstants.BackSlash + StringConstants.DataTilde;
}
