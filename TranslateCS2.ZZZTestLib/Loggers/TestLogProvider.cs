using Microsoft.Extensions.Logging;

namespace TranslateCS2.ZZZTestLib.Loggers;
internal class TestLogProvider<T> : ITestLogProvider {
    private readonly ILogger<T> logger;

    public int LogTraceCount { get; private set; }

    public bool HasLoggedTrace => this.LogTraceCount > 0;

    public int LogInfoCount { get; private set; }

    public bool HasLoggedInfo => this.LogInfoCount > 0;

    public int LogErrorCount { get; private set; }

    public bool HasLoggedError => this.LogErrorCount > 0;

    public int LogCriticalCount { get; private set; }

    public bool HasLoggedCritical => this.LogCriticalCount > 0;

    public int LogWarningCount { get; private set; }

    public bool HasLoggedWarning => this.LogWarningCount > 0;

    public int DisplayErrorCount { get; private set; }

    public bool HasDisplayedError => this.DisplayErrorCount > 0;

    public TestLogProvider(ILogger<T> logger) {
        this.logger = logger;
    }

    public void DisplayError(object message) {
        this.DisplayErrorCount++;
        // no need to realize
    }

    public void LogCritical(string message) {
        this.logger.LogCritical(message);
        this.LogCriticalCount++;
    }

    public void LogError(string message) {
        this.logger.LogError(message);
        this.LogErrorCount++;
    }

    public void LogInfo(string message) {
        this.logger.LogInformation(message);
        this.LogInfoCount++;
    }

    public void LogTrace(string message) {
        this.logger.LogTrace(message);
        this.LogTraceCount++;
    }

    public void LogWarning(string message) {
        this.logger.LogWarning(message);
        this.LogWarningCount++;
    }
}
