using TranslateCS2.ZZZModTestLib;
using TranslateCS2.ZZZModTestLib.Containers;
using TranslateCS2.ZZZTestLib.Loggers;

namespace TranslateCS2.ZModTests;
[Collection("TestDataOK")]
public class LogSomething {
    private readonly TestDataProvider dataProvider;
    public LogSomething(TestDataProvider testDataProvider) {
        this.dataProvider = testDataProvider;
    }
    [Fact]
    public void LogTest() {
        ITestLogProvider logProvider = TestLogProviderFactory.GetTestLogProvider<LogSomething>();
        ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer(logProvider,
                                                                               userDataPath: this.dataProvider.DirectoryName);
        runtimeContainer.Logger.LogInfo(this.GetType(), "This is just a test:", [0]);
    }
}
