using System;
using System.Reflection;
using System.Runtime.CompilerServices;

using TranslateCS2.Inf.Loggers;

namespace TranslateCS2.ZZZTestLib.Loggers;
public static class TestLogProviderFactory {
    /// <summary>
    ///     creates a subdirectory named "Logs"
    ///     <br/>
    ///     <br/>
    ///     within that subdirectory, another subdirectory is created and named via <see cref="Type"/>s <see cref="MemberInfo.Name"/>
    ///     <br/>
    ///     <br/>
    ///     and should be used per method
    ///     <br/>
    ///     <br/>
    ///     cause one log-file per test-method is generated via <paramref name="callerMemberName"/>
    /// </summary>
    /// <typeparam name="T">
    ///     <see cref="Type"/> of Test-<see langword="class"/>
    /// </typeparam>
    /// <param name="callerMemberName">
    ///     leave this parameter
    /// </param>
    /// <returns>
    ///     an <see cref="ITestLogProvider"/> to use with <see cref="IMyLogger"/>
    /// </returns>
    public static ITestLogProvider GetTestLogProvider<T>([CallerMemberName] string callerMemberName = "TestMethod") {
        // parameter callerMemberName has to be preinitialized with TestMethod,
        // just for the case something goes wrong with the CallerMemberNameAttribute
        Microsoft.Extensions.Logging.ILogger<T> logger = TestLoggerInternal.GetLogger<T>(callerMemberName: callerMemberName);
        return new TestLogProvider<T>(logger);

    }
}
