using System;
using System.Runtime.CompilerServices;

namespace TranslateCS2.Inf.Loggers;
public interface IMyLogger {
    void LogTrace(Type type,
                  string message,
                  object?[]? messageParameters = null,
                  [CallerMemberName] string CallerMemberName = "",
                  [CallerLineNumber] int CallerLineNumber = 0);
    void LogInfo(Type type,
                 string message,
                 object?[]? messageParameters = null,
                 [CallerMemberName] string CallerMemberName = "",
                 [CallerLineNumber] int CallerLineNumber = 0);
    void LogWarning(Type type,
                    string message,
                    object?[]? messageParameters = null,
                    [CallerMemberName] string CallerMemberName = "",
                    [CallerLineNumber] int CallerLineNumber = 0);
    void LogError(Type type,
                  string message,
                  object?[]? messageParameters = null,
                  [CallerMemberName] string CallerMemberName = "",
                  [CallerLineNumber] int CallerLineNumber = 0);
    void LogCritical(Type type,
                     string message,
                     object?[]? messageParameters = null,
                     [CallerMemberName] string CallerMemberName = "",
                     [CallerLineNumber] int CallerLineNumber = 0);
    void DisplayError(object message);
}
