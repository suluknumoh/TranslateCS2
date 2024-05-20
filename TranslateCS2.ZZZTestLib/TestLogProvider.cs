using Microsoft.Extensions.Logging;

using TranslateCS2.Inf.Loggers;

namespace TranslateCS2.ZZZTestLib;
internal class TestLogProvider<T> : IMyLogProvider {
    private readonly ILogger<T> logger;

    public TestLogProvider(ILogger<T> logger) {
        this.logger = logger;
    }

    public void DisplayError(object message) {
        // no need to realize
    }

    public void LogCritical(string message) {
        this.logger.LogCritical(message);
    }

    public void LogError(string message) {
        this.logger.LogError(message);
    }

    public void LogInfo(string message) {
        this.logger.LogInformation(message);
    }

    public void LogTrace(string message) {
        this.logger.LogTrace(message);
    }

    public void LogWarning(string message) {
        this.logger.LogWarning(message);
    }
}
