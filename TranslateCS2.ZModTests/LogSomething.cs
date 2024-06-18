using TranslateCS2.ZModTests.TestHelpers.Models;
using TranslateCS2.ZModTests.TestHelpers.Containers;
using TranslateCS2.ZZZTestLib.Loggers;

using Xunit;

namespace TranslateCS2.ZModTests;
public class LogSomething : AProvidesTestDataOk {
    public LogSomething(TestDataProvider testDataProvider) : base(testDataProvider) { }

    [Fact]
    public void LogTest() {
        ITestLogProvider logProvider = TestLogProviderFactory.GetTestLogProvider<LogSomething>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(logProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        runtimeContainer.Logger.LogInfo(this.GetType(), "This is just a test:", [0]);
    }
}
