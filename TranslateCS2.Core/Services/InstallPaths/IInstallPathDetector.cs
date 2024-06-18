using TranslateCS2.Inf.Interfaces;

namespace TranslateCS2.Core.Services.InstallPaths;
public interface IInstallPathDetector : ILocFileDirectoryProvider {
    string InstallPath { get; }
}
