using TranslateCS2.ZZZModTestLib.Containers;
using TranslateCS2.ZZZTestLib.Loggers;

namespace TranslateCS2.ZModDevHelper;
public class LogSomething {
    [Fact]
    public void LogTest() {
        ITestLogProvider logProvider = TestLogProviderFactory.GetTestLogProvider<LogSomething>();
        ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer(logProvider);
        runtimeContainer.Logger.LogInfo(this.GetType(), "This is just a test:", [0]);
    }
}
