using System;
using System.IO;

using TranslateCS2.Inf;

using Xunit;

namespace TranslateCS2.ZTests.Inf;
public class PathsTests {
    [Fact]
    public void CtorNullTest() {
        Assert.Throws<ArgumentNullException>(() => new Paths(true, null));
    }
    [Theory]
    [InlineData("sdp", "udp", false)]
    [InlineData("sdp", "udp", true)]
    public void CtorTest(string streamingDataPath,
                         string userDataPath,
                         bool createIfNotExists) {
        // do not test null for userDataPath to test the fallback!!!
        Paths paths = new Paths(createIfNotExists,
                                streamingDataPath,
                                userDataPath);
        Assert.NotNull(paths);
        string expectedSDP = $"{streamingDataPath}{StringConstants.ForwardSlash}";
        Assert.EndsWith(expectedSDP, paths.StreamingDataPath);
        string expectedSDDP = expectedSDP + $"{StringConstants.DataTilde}{StringConstants.ForwardSlash}";
        Assert.EndsWith(expectedSDDP, paths.StreamingDatasDataPath);
        string expectedUDP = $"{userDataPath}{StringConstants.ForwardSlash}";
        Assert.EndsWith(expectedUDP, paths.UserDataPath);
        string expectedMDPG = expectedUDP + $"{ModConstants.ModsData}{StringConstants.ForwardSlash}";
        Assert.EndsWith(expectedMDPG, paths.ModsDataPathGeneral);
        string expectedMDPS = expectedMDPG + $"{ModConstants.Name}{StringConstants.ForwardSlash}";
        Assert.EndsWith(expectedMDPS, paths.ModsDataPathSpecific);
        // is not created!
        Assert.False(Directory.Exists(streamingDataPath));
        string? resultMDPS = paths.TryToGetModsPath();
        if (createIfNotExists) {
            Assert.True(Directory.Exists(paths.UserDataPath));
            Assert.True(Directory.Exists(paths.ModsDataPathGeneral));
            Assert.True(Directory.Exists(paths.ModsDataPathSpecific));
            Assert.NotNull(resultMDPS);
            Assert.EndsWith(expectedMDPS, resultMDPS);
            Directory.Delete(userDataPath, true);
        } else {
            Assert.False(Directory.Exists(paths.UserDataPath));
            Assert.False(Directory.Exists(paths.ModsDataPathGeneral));
            Assert.False(Directory.Exists(paths.ModsDataPathSpecific));
            Assert.Null(resultMDPS);
        }
    }
    [Theory]
    [InlineData(null, null)]
    [InlineData("null", "null")]
    [InlineData("C:/Test", "C:/Test")]
    [InlineData("C:\\Test", "C:/Test")]
    [InlineData("\\", "/")]
    [InlineData("/", "/")]
    public void NormalizeUnixTest(string? input, string? expected) {
        string? result = Paths.NormalizeUnix(input);
        Assert.Equal(expected, result);
    }
    [Theory]
    [InlineData(null, null)]
    [InlineData("null", "null")]
    [InlineData("C:/Test", "C:\\Test")]
    [InlineData("C:\\Test", "C:\\Test")]
    [InlineData("\\", "\\")]
    [InlineData("/", "\\")]
    public void NormalizeWindowsTest(string? input, string? expected) {
        string? result = Paths.NormalizeWindows(input);
        Assert.Equal(expected, result);
    }
    [Fact]
    public void GetFallbackUserDataPathUnixFormatTest() {
        string result = Paths.GetFallbackUserDataPathUnixFormat();
        Assert.EndsWith("Low/Colossal Order/Cities Skylines II", result);
    }
}
