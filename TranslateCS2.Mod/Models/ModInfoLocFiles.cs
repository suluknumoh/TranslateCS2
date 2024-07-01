using System;
using System.Collections.Generic;
using System.IO;

using TranslateCS2.Mod.Enums;

namespace TranslateCS2.Mod.Models;
internal class ModInfoLocFiles {
    public FlavorSourceInfo FlavorSourceInfo { get; }
    public IEnumerable<FileInfo> FileInfos { get; }
    public ModInfoLocFiles(string id,
                           string name,
                           Version version,
                           bool isLocal,
                           FlavorSourceTypes flavorSourceType,
                           IEnumerable<FileInfo> fileInfos) {
        this.FlavorSourceInfo = new FlavorSourceInfo(id,
                                                     name,
                                                     version,
                                                     isLocal,
                                                     flavorSourceType);
        this.FileInfos = fileInfos;
    }
}
