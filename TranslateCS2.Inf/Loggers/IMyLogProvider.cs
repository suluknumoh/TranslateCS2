namespace TranslateCS2.Inf.Loggers;
public interface IMyLogProvider {
    void LogTrace(string message);
    void LogInfo(string message);
    void LogError(string message);
    void LogCritical(string message);
    void LogWarning(string message);
    void DisplayError(object message);
}
