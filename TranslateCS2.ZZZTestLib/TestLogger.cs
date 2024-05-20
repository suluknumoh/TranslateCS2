using System.Runtime.CompilerServices;

using TranslateCS2.Inf.Loggers;

namespace TranslateCS2.ZZZTestLib;
public static class TestLogger {
    public static IMyLogProvider GetTestLogProvider<T>([CallerMemberName] string callerMemberName = "") {
        Microsoft.Extensions.Logging.ILogger<T> logger = TestLoggerInternal.GetLogger<T>(callerMemberName: callerMemberName);
        return new TestLogProvider<T>(logger);

    }
}
