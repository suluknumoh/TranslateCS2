using System.Collections.Generic;
using System.IO;

using TranslateCS2.Mod.Enums;

namespace TranslateCS2.Mod.Models;
internal class ModInfoLocFiles {
    public string Id { get; }
    public string Name { get; }
    public FlavorSourceTypes FlavorSourceType { get; }
    public IEnumerable<FileInfo> FileInfos { get; }
    public ModInfoLocFiles(string id,
                           string name,
                           IEnumerable<FileInfo> fileInfos,
                           FlavorSourceTypes flavorSourceType) {
        this.Id = id;
        this.Name = name;
        this.FileInfos = fileInfos;
        this.FlavorSourceType = flavorSourceType;
    }
}
