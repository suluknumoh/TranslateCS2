using Colossal.Logging;

using TranslateCS2.Inf.Attributes;
using TranslateCS2.Inf.Loggers;

namespace TranslateCS2.Mod.Loggers;
[MyExcludeClassFromCoverage]
internal class ModLogProvider : IMyLogProvider {
    private readonly ILog logger;
    public ModLogProvider(ILog logger) {
        this.logger = logger;
    }

    public void LogCritical(string message) {
        this.logger.Critical(message);
    }

    public void LogError(string message) {
        this.logger.Error(message);
    }

    public void LogInfo(string message) {
        this.logger.Info(message);
    }

    public void LogTrace(string message) {
        this.logger.Trace(message);
    }

    public void LogWarning(string message) {
        this.logger.Warn(message);
    }

    public void DisplayError(object message) {
        this.logger.SetShowsErrorsInUI(true);
        this.logger.Error(message);
        this.logger.SetShowsErrorsInUI(false);
    }
}
