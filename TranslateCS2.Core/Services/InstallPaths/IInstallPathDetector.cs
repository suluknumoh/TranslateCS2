namespace TranslateCS2.Core.Services.InstallPaths;
public interface IInstallPathDetector {
    string? InstallPath { get; }
    bool Detect();
    static IInstallPathDetector Instance { get; } = new InstallPathDetector();
}
