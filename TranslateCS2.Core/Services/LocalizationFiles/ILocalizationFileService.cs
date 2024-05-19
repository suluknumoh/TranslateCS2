using System.Collections.Generic;
using System.IO;

using TranslateCS2.Core.Sessions;

namespace TranslateCS2.Core.Services.LocalizationFiles;
public interface ILocalizationFileService {
    IEnumerable<FileInfo> GetLocalizationFiles();
    LocalizationFile GetLocalizationFile(FileInfo fileInfo);
}
