using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

using TranslateCS2.Inf;

namespace TranslateCS2.ZTests.Inf;
public class IndexCountHelperTests {
    [Theory]
    [InlineData("existing_only.json", 5, 5, 0)]
    [InlineData("existing_only.json", 0, 5, 0)]
    [InlineData("new_only.json", 5, 10, 0)]
    [InlineData("existing_and_new.json", 5, 10, 0)]
    [InlineData("existing_and_new_error.json", 5, 5, 1)]
    public async Task FillIndexCountsFromLocalizationDictionaryTest(string fileName,
                                                                    int preInitCounts,
                                                                    int expectedCounts,
                                                                    int expectedErrorCount) {
        string key = "Chirper.DIRTY_WATER";
        string path = Path.Combine("Assets", "Inf", "IndexCountHelperTestFiles", fileName);
        using FileStream stream = File.OpenRead(path);
        Dictionary<string, string>? localizationDictionary = await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream);
        Assert.NotNull(localizationDictionary);
        Dictionary<string, int> indexCounts = new Dictionary<string, int> {
            { key, preInitCounts }
        };
        IList<string> errors = [];
        IndexCountHelper.FillIndexCountsFromLocalizationDictionary(localizationDictionary,
                                                                   indexCounts,
                                                                   errors);
        Assert.Equal(expectedErrorCount, errors.Count);
        Assert.Contains(key, indexCounts.Keys);
        int actual = indexCounts[key];
        Assert.Equal(expectedCounts, actual);
    }
}
