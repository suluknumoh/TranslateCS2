using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

using TranslateCS2.Inf;
using TranslateCS2.Inf.Keyz;

namespace TranslateCS2.ZTests.Inf;
public class IndexCountHelperTests {
    [Theory]
    [InlineData("existing_only.json", 5, 5)]
    [InlineData("new_for_existing.json", 5, 11)]
    [InlineData("existing_and_new.json", 5, 11)]
    [InlineData("existing_and_new_error.json", 5, 11)]
    [InlineData("complete_new.json", 0, 5)]
    [InlineData("complete_new_error.json", 0, 5)]
    [InlineData("complete_new_mixed.json", 0, 6)]
    public async Task FillIndexCountsAndAutocorrectTest(string fileName,
                                                        int preInitCounts,
                                                        int expectedCounts) {
        string key = "Chirper.DIRTY_WATER";
        IDictionary<string, string>? localizationDictionary = await this.GetDictionary(fileName);
        Assert.NotNull(localizationDictionary);
        Dictionary<string, int> indexCounts = [];
        if (preInitCounts > 0) {
            indexCounts.Add(key, preInitCounts);
        }
        IndexCountHelper.FillIndexCountsAndAutocorrect(localizationDictionary,
                                                       indexCounts);
        Assert.Single(indexCounts);
        Assert.Contains(key, indexCounts.Keys);
        int actual = indexCounts[key];
        Assert.Equal(expectedCounts, actual);
    }

    [Theory]
    [InlineData("existing_only.json", 5)]
    // INFO: AutoCorrect requires all corresponding Key-Value-Pairs; so here six is the correct expectation
    [InlineData("new_for_existing.json", 6)]
    [InlineData("existing_and_new.json", 11)]
    [InlineData("existing_and_new_error.json", 11)]
    [InlineData("complete_new.json", 5)]
    [InlineData("complete_new_error.json", 5)]
    [InlineData("complete_new_mixed.json", 6)]
    public async Task AutoCorrectTest(string fileName,
                                      int expectedCounts) {
        IDictionary<string, string>? localizationDictionary = await this.GetDictionary(fileName);
        Assert.NotNull(localizationDictionary);
        IList<IMyKey> localizations = [];
        foreach (KeyValuePair<string, string> entry in localizationDictionary) {
            localizations.Add(new MyKey(entry.Key));
        }
        // TODO: IMyKeyProvider
        //IndexCountHelper.AutoCorrect(localizations);
        IEnumerable<IMyKey> orderedIndexedValues =
            localizations
                .Where(item => item.IsIndexed)
                .OrderBy(item => item.Index);
        Assert.Equal(expectedCounts, orderedIndexedValues.Count());
        int index = 0;
        foreach (IMyKey entry in orderedIndexedValues) {
            Assert.Equal(index, entry.Index);
            index++;
        }
        int expectedIndex = expectedCounts--;
        Assert.Equal(expectedIndex, index);
    }
    [Theory]
    [InlineData("TheNewKey", 0, "TheNewKey:0")]
    [InlineData("TheNewKey", 10, "TheNewKey:10")]
    [InlineData("TheNewKey", 100, "TheNewKey:100")]
    [InlineData("TheNewKey", 1000, "TheNewKey:1000")]
    [InlineData("TheNewKey", 10000, "TheNewKey:10000")]
    public void BuildNewKeyTest(string key,
                                int newIndex,
                                string expectation) {
        string result = IndexCountHelper.BuildNewKey(key, newIndex);
        Assert.Equal(expectation, result);
    }
    [Theory]
    [InlineData("TheNewKey", "TheNewKey")]
    [InlineData("TheNewKey:", "TheNewKey:")]
    [InlineData("TheNewKey:-0", "TheNewKey:-0")]
    [InlineData("TheNewKey:+0", "TheNewKey:+0")]
    [InlineData("TheNewKey:0", "TheNewKey")]
    [InlineData("TheNewKey:10", "TheNewKey")]
    [InlineData("TheNewKey:100", "TheNewKey")]
    [InlineData("TheNewKey:1000", "TheNewKey")]
    [InlineData("TheNewKey:10000", "TheNewKey")]
    public void GetCountKeyFromKeyTest(string key,
                                       string expectation) {
        string result = IndexCountHelper.GetCountKeyFromKey(key);
        Assert.Equal(expectation, result);
    }
    [Theory]
    [InlineData("TheNewKey", 0)]
    [InlineData("TheNewKey:", 0)]
    [InlineData("TheNewKey:-0", 0)]
    [InlineData("TheNewKey:-1", 0)]
    [InlineData("TheNewKey:+0", 0)]
    [InlineData("TheNewKey:+1", 0)]
    [InlineData("TheNewKey:0", 0)]
    [InlineData("TheNewKey:10", 10)]
    [InlineData("TheNewKey:100", 100)]
    [InlineData("TheNewKey:1000", 1000)]
    [InlineData("TheNewKey:10000", 10000)]
    public void GetIndexFromKeyTest(string key,
                                    int expectation) {
        int result = IndexCountHelper.GetIndexFromKey(key);
        Assert.Equal(expectation, result);
    }
    [Theory]
    [InlineData(0, true, 0)]
    [InlineData(1, true, 0)]
    [InlineData(2, true, 0)]
    [InlineData(3, true, 0)]
    [InlineData(4, true, 0)]
    [InlineData(5, true, 0)]
    [InlineData(6, true, 0)]
    [InlineData(7, true, 0)]
    [InlineData(8, true, 0)]
    [InlineData(9, true, 0)]
    [InlineData(10, true, 0)]
    [InlineData(11, false, 10)]
    public async Task ValidateForKeyTest(int index, bool isValid, int nextFreeIndex) {
        string key = "Chirper.DIRTY_WATER";
        string fileName = "validate_for_key.json";
        IDictionary<string, string>? localizationDictionary = await this.GetDictionary(fileName);
        Assert.NotNull(localizationDictionary);
        IList<IMyKey> localizations = [];
        foreach (KeyValuePair<string, string> entry in localizationDictionary) {
            localizations.Add(new MyKey(entry.Key));
        }
        string newKey = IndexCountHelper.BuildNewKey(key, index);
        // TODO: IMyKeyProvider
        //IndexCountHelperValidationResult result = IndexCountHelper.ValidateForKey(localizations, newKey);
        //Assert.NotNull(result);
        //Assert.Equal(isValid, result.IsValid);
        //Assert.Equal(nextFreeIndex, result.NextFreeIndex);
        //if (isValid) {
        //    Assert.Null(result.Erroneous);
        //} else {
        //    Assert.Equal(newKey, result.Erroneous);
        //}
    }
    private async Task<Dictionary<string, string>?> GetDictionary(string fileName) {
        string path = Path.Combine("Assets", "Inf", "IndexCountHelperTestFiles", fileName);
        using FileStream stream = File.OpenRead(path);
        return await JsonSerializer.DeserializeAsync<Dictionary<string, string>>(stream);
    }
}
