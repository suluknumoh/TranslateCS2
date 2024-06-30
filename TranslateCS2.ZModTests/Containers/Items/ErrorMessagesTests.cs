using System.Collections.Generic;

using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.ZModTests.TestHelpers.Containers;
using TranslateCS2.ZModTests.TestHelpers.Models;
using TranslateCS2.ZZZTestLib.Loggers;

using Xunit;

namespace TranslateCS2.ZModTests.Containers.Items;
public class ErrorMessagesTests : AProvidesTestDataOk {
    public ErrorMessagesTests(TestDataProvider testDataProvider) : base(testDataProvider) { }
    [Fact]
    public void DisplayErrorMessageFailedToGenerateJsonTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ErrorMessagesTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        runtimeContainer.Init();
        ErrorMessages errorMessages = runtimeContainer.ErrorMessages;
        errorMessages.DisplayErrorMessageFailedToGenerateJson();
        Assert.Equal(1, testLogProvider.LogErrorCount);
        Assert.Equal(1, testLogProvider.DisplayErrorCount);
    }
    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 1)]
    public void DisplayErrorMessageForErroneousTests(bool missing, int expectedLogCounts) {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ErrorMessagesTests>();
        ModTestRuntimeContainer runtimeContainer = ModTestRuntimeContainer.Create(testLogProvider,
                                                                                  userDataPath: this.dataProvider.DirectoryName);
        runtimeContainer.Init();
        ErrorMessages errorMessages = runtimeContainer.ErrorMessages;
        IList<FlavorSource> erroneous = [];
        errorMessages.DisplayErrorMessageForErroneous(erroneous, missing);
        Assert.Equal(expectedLogCounts, testLogProvider.DisplayErrorCount);
        Assert.Equal(expectedLogCounts, testLogProvider.LogErrorCount);
    }
}
