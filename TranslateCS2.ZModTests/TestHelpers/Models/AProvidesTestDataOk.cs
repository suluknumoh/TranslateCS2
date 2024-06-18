using Xunit;

namespace TranslateCS2.ZModTests.TestHelpers.Models;
/// <summary>
///     <see langword="abstract"/> <see langword="class"/> that provides non corrupt test-data via <see cref="dataProvider"/>
/// </summary>
[Collection("TestDataOK")]
public abstract class AProvidesTestDataOk {
    protected readonly TestDataProvider dataProvider;
    public AProvidesTestDataOk(TestDataProvider testDataProvider) {
        this.dataProvider = testDataProvider;
        this.dataProvider.GenerateData();
    }
}
