using TranslateCS2.Inf.Loggers;

namespace TranslateCS2.ZZZTestLib.Loggers;
public interface ITestLogProvider : IMyLogProvider {
    int LogTraceCount { get; }
    bool HasLoggedTrace { get; }
    int LogInfoCount { get; }
    bool HasLoggedInfo { get; }
    int LogErrorCount { get; }
    bool HasLoggedError { get; }
    int LogCriticalCount { get; }
    bool HasLoggedCritical { get; }
    int LogWarningCount { get; }
    bool HasLoggedWarning { get; }
    int DisplayErrorCount { get; }
    bool HasDisplayedError { get; }
}
