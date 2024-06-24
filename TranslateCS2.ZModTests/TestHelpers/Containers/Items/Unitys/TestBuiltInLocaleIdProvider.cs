using System;
using System.Collections.Generic;
using System.IO;

using TranslateCS2.Inf;
using TranslateCS2.Mod.Interfaces;

namespace TranslateCS2.ZModTests.TestHelpers.Containers.Items.Unitys;
internal class TestBuiltInLocaleIdProvider : IBuiltInLocaleIdProvider {
    private readonly Paths paths;
    public TestBuiltInLocaleIdProvider(Paths paths) {
        this.paths = paths;
    }
    public IReadOnlyList<string> GetBuiltInLocaleIds() {
        List<string> localeIds = [];
        // has to end with a forward-slash
        string path = $"{this.paths.StreamingDatasDataPath}";
        IEnumerable<string> locFiles = Directory.EnumerateFiles(path, ModConstants.LocSearchPattern);
        foreach (string locFile in locFiles) {
            string locale =
                locFile
                .Replace(path, String.Empty)
                .Replace(ModConstants.LocExtension, String.Empty);
            localeIds.Add(locale);
        }
        return localeIds;
    }
}
