using TranslateCS2.Inf.Loggers;
using TranslateCS2.ZZZTestLib;

namespace TranslateCS2.ZModTests;
public class LogSomething {
    [Fact]
    public void LogTest() {
        IMyLogProvider logProvider = TestLogger.GetTestLogProvider<LogSomething>();
        ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer(logProvider);
        runtimeContainer.Logger.LogInfo(this.GetType(), "This is just a test:", [0]);
    }
}
