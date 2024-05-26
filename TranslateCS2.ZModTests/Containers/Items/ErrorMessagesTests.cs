using System.Collections.Generic;

using TranslateCS2.Mod.Containers.Items;
using TranslateCS2.ZZZModTestLib;
using TranslateCS2.ZZZModTestLib.Containers;
using TranslateCS2.ZZZTestLib.Loggers;

using Xunit;

namespace TranslateCS2.ZModTests.Containers.Items;
[Collection("TestDataOK")]
public class ErrorMessagesTests {
    private readonly TestDataProvider dataProvider;
    public ErrorMessagesTests(TestDataProvider testDataProvider) {
        this.dataProvider = testDataProvider;
    }
    [Fact]
    public void DisplayErrorMessageFailedToGenerateJsonTest() {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ErrorMessagesTests>();
        ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer(testLogProvider,
                                                                               userDataPath: this.dataProvider.DirectoryName);
        ErrorMessages errorMessages = new ErrorMessages(runtimeContainer);
        errorMessages.DisplayErrorMessageFailedToGenerateJson();
        Assert.Equal(1, testLogProvider.LogErrorCount);
        Assert.Equal(1, testLogProvider.DisplayErrorCount);
    }
    [Theory]
    [InlineData(true, 1)]
    [InlineData(false, 1)]
    public void DisplayErrorMessageForErroneousTests(bool missing, int expectedLogCounts) {
        ITestLogProvider testLogProvider = TestLogProviderFactory.GetTestLogProvider<ErrorMessagesTests>();
        ModTestRuntimeContainer runtimeContainer = new ModTestRuntimeContainer(testLogProvider,
                                                                               userDataPath: this.dataProvider.DirectoryName);
        ErrorMessages errorMessages = new ErrorMessages(runtimeContainer);
        IList<TranslationFile> erroneous = [];
        errorMessages.DisplayErrorMessageForErroneous(erroneous, missing);
        Assert.Equal(expectedLogCounts, testLogProvider.DisplayErrorCount);
        Assert.Equal(expectedLogCounts, testLogProvider.LogErrorCount);
    }
}
