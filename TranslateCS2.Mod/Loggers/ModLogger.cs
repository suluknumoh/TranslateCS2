using System;
using System.Runtime.CompilerServices;
using System.Text;

using TranslateCS2.Inf.Loggers;
namespace TranslateCS2.Mod.Loggers;
internal class ModLogger : IMyLogger {
    private static readonly string format = "in {0}.{1} at line {2}:";
    private readonly IMyLogProvider logProvider;
    public ModLogger(IMyLogProvider logProvider) {
        this.logProvider = logProvider;
    }
    public void LogTrace(Type type,
                         string message,
                         object?[]? messageParameters = null,
                         [CallerMemberName] string CallerMemberName = "",
                         [CallerLineNumber] int CallerLineNumber = 0) {
        string messageToLog = BuildLogMessage(type,
                                              message,
                                              messageParameters,
                                              CallerMemberName,
                                              CallerLineNumber);
        this.logProvider.LogTrace(messageToLog);
    }
    public void LogInfo(Type type,
                        string message,
                        object?[]? messageParameters = null,
                        [CallerMemberName] string CallerMemberName = "",
                        [CallerLineNumber] int CallerLineNumber = 0) {
        string messageToLog = BuildLogMessage(type,
                                              message,
                                              messageParameters,
                                              CallerMemberName,
                                              CallerLineNumber);
        this.logProvider.LogInfo(messageToLog);
    }
    public void LogWarning(Type type,
                           string message,
                           object?[]? messageParameters = null,
                           [CallerMemberName] string CallerMemberName = "",
                           [CallerLineNumber] int CallerLineNumber = 0) {
        string messageToLog = BuildLogMessage(type,
                                              message,
                                              messageParameters,
                                              CallerMemberName,
                                              CallerLineNumber);
        this.logProvider.LogWarning(messageToLog);
    }
    public void LogError(Type type,
                         string message,
                         object?[]? messageParameters = null,
                         [CallerMemberName] string CallerMemberName = "",
                         [CallerLineNumber] int CallerLineNumber = 0) {
        string messageToLog = BuildLogMessage(type,
                                              message,
                                              messageParameters,
                                              CallerMemberName,
                                              CallerLineNumber);
        this.logProvider.LogError(messageToLog);
    }
    public void LogCritical(Type type,
                            string message,
                            object?[]? messageParameters = null,
                            [CallerMemberName] string CallerMemberName = "",
                            [CallerLineNumber] int CallerLineNumber = 0) {
        string messageToLog = BuildLogMessage(type,
                                              message,
                                              messageParameters,
                                              CallerMemberName,
                                              CallerLineNumber);
        this.logProvider.LogCritical(messageToLog);
    }
    private static string BuildLogMessage(Type type,
                                          string message,
                                          object?[]? messageParameters,
                                          string CallerMemberName,
                                          int CallerLineNumber) {
        string preFormat = String.Format(format, type.FullName, CallerMemberName, CallerLineNumber);
        StringBuilder messageBuilder = new StringBuilder();
        messageBuilder.AppendLine(preFormat);
        messageBuilder.AppendLine(message);
        if (messageParameters != null) {
            foreach (object? parameter in messageParameters) {
                if (parameter is null) {
                    continue;
                }
                messageBuilder.AppendLine(parameter.ToString());
            }
        }
        return messageBuilder.ToString();
    }

    public void DisplayError(object message) {
        this.logProvider.DisplayError(message);
    }
}
