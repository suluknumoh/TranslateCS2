using System;
using System.Runtime.CompilerServices;
using System.Text;

using Colossal.Logging;

namespace TranslateCS2.Mod.Loggers;

internal static class LogExtensions {
    private static readonly string Format = "in {0}.{1} at line {2}:\r\n";
    public static void LogError(this ILog logger,
                                Type type,
                                string message,
                                object[]? messageParameters = null,
                                [CallerMemberName] string CallerMemberName = "",
                                [CallerLineNumber] int CallerLineNumber = 0) {
        string messageToLog = BuildLogMessage(type,
                                              message,
                                              messageParameters,
                                              CallerMemberName,
                                              CallerLineNumber);
        logger?.Error(messageToLog);
    }
    public static void LogInfo(this ILog logger,
                               Type type,
                               string message,
                               object[]? messageParameters = null,
                               [CallerMemberName] string CallerMemberName = "",
                               [CallerLineNumber] int CallerLineNumber = 0) {
        string messageToLog = BuildLogMessage(type,
                                              message,
                                              messageParameters,
                                              CallerMemberName,
                                              CallerLineNumber);
        logger?.Info(messageToLog);
    }
    public static void LogCritical(this ILog logger,
                                   Type type,
                                   string message,
                                   object[]? messageParameters = null,
                                   [CallerMemberName] string CallerMemberName = "",
                                   [CallerLineNumber] int CallerLineNumber = 0) {
        string messageToLog = BuildLogMessage(type,
                                              message,
                                              messageParameters,
                                              CallerMemberName,
                                              CallerLineNumber);
        logger?.Critical(messageToLog);
    }
    private static string BuildLogMessage(Type type,
                                          string message,
                                          object[]? messageParameters,
                                          string CallerMemberName,
                                          int CallerLineNumber) {
        string preFormat = String.Format(Format, type.FullName, CallerMemberName, CallerLineNumber);
        string formattedMessage = String.Format(message, messageParameters ?? Array.Empty<object>());
        StringBuilder messageBuilder = new StringBuilder();
        messageBuilder.Append(preFormat);
        messageBuilder.AppendLine(formattedMessage);
        return messageBuilder.ToString();
    }
}
