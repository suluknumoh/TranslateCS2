using TranslateCS2.Mod.Loggers;
using TranslateCS2.ZZZTestLib.Loggers;

using Xunit;

namespace TranslateCS2.ZModTests.Loggers;
public class ModLoggerTests {
    private readonly string message = "test";
    [Theory]
    [InlineData("obj")]
    [InlineData(null)]
    public void LogTraceTest(object? parameter) {
        ITestLogProvider logProvider = TestLogProviderFactory.GetTestLogProvider<ModLoggerTests>();
        ModLogger logger = new ModLogger(logProvider);
        logger.LogTrace(this.GetType(), this.message, [parameter]);
        Assert.True(logProvider.HasLoggedTrace);
        Assert.Equal(1, logProvider.LogTraceCount);
        //
        Assert.False(logProvider.HasLoggedInfo);
        Assert.False(logProvider.HasLoggedWarning);
        Assert.False(logProvider.HasLoggedError);
        Assert.False(logProvider.HasLoggedCritical);
        Assert.False(logProvider.HasDisplayedError);
    }
    [Theory]
    [InlineData("obj")]
    [InlineData(null)]
    public void LogInfoTest(object? parameter) {
        ITestLogProvider logProvider = TestLogProviderFactory.GetTestLogProvider<ModLoggerTests>();
        ModLogger logger = new ModLogger(logProvider);
        logger.LogInfo(this.GetType(), this.message, [parameter]);
        Assert.True(logProvider.HasLoggedInfo);
        Assert.Equal(1, logProvider.LogInfoCount);
        //
        Assert.False(logProvider.HasLoggedTrace);
        Assert.False(logProvider.HasLoggedWarning);
        Assert.False(logProvider.HasLoggedError);
        Assert.False(logProvider.HasLoggedCritical);
        Assert.False(logProvider.HasDisplayedError);
    }
    [Theory]
    [InlineData("obj")]
    [InlineData(null)]
    public void LogWarningTest(object? parameter) {
        ITestLogProvider logProvider = TestLogProviderFactory.GetTestLogProvider<ModLoggerTests>();
        ModLogger logger = new ModLogger(logProvider);
        logger.LogWarning(this.GetType(), this.message, [parameter]);
        Assert.True(logProvider.HasLoggedWarning);
        Assert.Equal(1, logProvider.LogWarningCount);
        //
        Assert.False(logProvider.HasLoggedInfo);
        Assert.False(logProvider.HasLoggedTrace);
        Assert.False(logProvider.HasLoggedError);
        Assert.False(logProvider.HasLoggedCritical);
        Assert.False(logProvider.HasDisplayedError);
    }
    [Theory]
    [InlineData("obj")]
    [InlineData(null)]
    public void LogErrorTest(object? parameter) {
        ITestLogProvider logProvider = TestLogProviderFactory.GetTestLogProvider<ModLoggerTests>();
        ModLogger logger = new ModLogger(logProvider);
        logger.LogError(this.GetType(), this.message, [parameter]);
        Assert.True(logProvider.HasLoggedError);
        Assert.Equal(1, logProvider.LogErrorCount);
        //
        Assert.False(logProvider.HasLoggedInfo);
        Assert.False(logProvider.HasLoggedTrace);
        Assert.False(logProvider.HasLoggedWarning);
        Assert.False(logProvider.HasLoggedCritical);
        Assert.False(logProvider.HasDisplayedError);
    }
    [Theory]
    [InlineData("obj")]
    [InlineData(null)]
    public void LogCriticalTest(object? parameter) {
        ITestLogProvider logProvider = TestLogProviderFactory.GetTestLogProvider<ModLoggerTests>();
        ModLogger logger = new ModLogger(logProvider);
        logger.LogCritical(this.GetType(), this.message, [parameter]);
        Assert.True(logProvider.HasLoggedCritical);
        Assert.Equal(1, logProvider.LogCriticalCount);
        //
        Assert.False(logProvider.HasLoggedInfo);
        Assert.False(logProvider.HasLoggedTrace);
        Assert.False(logProvider.HasLoggedWarning);
        Assert.False(logProvider.HasLoggedError);
        Assert.False(logProvider.HasDisplayedError);
    }
    [Fact]
    public void DisplayErrorTest() {
        ITestLogProvider logProvider = TestLogProviderFactory.GetTestLogProvider<ModLoggerTests>();
        ModLogger logger = new ModLogger(logProvider);
        logger.DisplayError(this.message);
        Assert.True(logProvider.HasDisplayedError);
        Assert.Equal(1, logProvider.DisplayErrorCount);
        //
        Assert.False(logProvider.HasLoggedInfo);
        Assert.False(logProvider.HasLoggedTrace);
        Assert.False(logProvider.HasLoggedWarning);
        Assert.False(logProvider.HasLoggedError);
        Assert.False(logProvider.HasLoggedCritical);
    }
}
