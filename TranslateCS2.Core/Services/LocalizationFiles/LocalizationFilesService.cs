using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Helpers;
using TranslateCS2.Core.Services.InstallPaths;
using TranslateCS2.Core.Sessions;
using TranslateCS2.Inf;

namespace TranslateCS2.Core.Services.LocalizationFiles;
internal class LocalizationFilesService : ILocalizationFilesService {
    private readonly bool _skipWorkAround = AppConfigurationManager.SkipWorkAround;
    private readonly InstallPathDetector _installPathDetector;

    public LocalizationFilesService(InstallPathDetector installPathDetector) {
        this._installPathDetector = installPathDetector;
    }
    public IEnumerable<FileInfo> GetLocalizationFiles() {
        string installPath = this._installPathDetector.DetectInstallPath();
        string locLocation = Path.Combine(installPath, "Cities2_Data", "StreamingAssets", "Data~");
        DirectoryInfo loc = new DirectoryInfo(locLocation);
        return loc.EnumerateFiles(ModConstants.LocSearchPattern);
    }
    /// <seealso href="https://github.com/grotaclas/PyHelpersForPDXWikis/blob/main/cs2/localization.py">
    public LocalizationFile GetLocalizationFile(FileInfo fileInfo) {
        using Stream stream = File.OpenRead(fileInfo.FullName);
        return GetLocalizationFile(fileInfo, stream);
    }
    /// <seealso href="https://github.com/grotaclas/PyHelpersForPDXWikis/blob/main/cs2/localization.py">
    private static LocalizationFile GetLocalizationFile(FileInfo fileInfo, Stream stream) {
        short fileHeader = ReadInt16(stream);
        string localeNameEN = ReadString(stream);
        string localeNameID = ReadString(stream);
        string localeNameLocalized = ReadString(stream);
        LocalizationFile localizationFile = new LocalizationFile(fileInfo.Name, fileHeader, localeNameEN, localeNameID, localeNameLocalized);
        ReadLocalizationFilesLocalizations(stream, localizationFile);
        ReadLocalizationFilesIndices(stream, localizationFile);
        return localizationFile;
    }

    private static void ReadLocalizationFilesIndices(Stream stream, ILocalizationFile localizationFile) {
        int indexCount = ReadInt32(stream);
        for (int i = 0; i < indexCount; i++) {
            string key = ReadString(stream);
            int val = ReadInt32(stream);
            localizationFile.Indices.Add(new KeyValuePair<string, int>(key, val));
        }
    }

    private static void ReadLocalizationFilesLocalizations(Stream stream, ILocalizationFile localizationFile) {
        int localizationCount = ReadInt32(stream);
        for (int i = 0; i < localizationCount; i++) {
            string key = ReadString(stream);
            string value = ReadString(stream);
            ILocalizationEntry originLocalizationKey = new LocalizationEntry(key, value, null, false);
            localizationFile.Localizations.Add(originLocalizationKey);
        }
    }

    public async Task WriteLocalizationFileDirect(ILocalizationFile localizationFile,
                                                  Stream? streamParameter = null) {
        await Task.Factory.StartNew(() => {
            FileInfo fileInfo = this.GetLocalizationFileInfo(localizationFile.FileName);
            WorkAround workAround = new WorkAround(this._skipWorkAround);
            workAround.Start(fileInfo);
            //
            // write new file with this apps logic
            using Stream stream = streamParameter ?? File.OpenWrite(fileInfo.FullName);
            WriteInt16(stream, localizationFile.FileHeader);
            WriteString(stream, localizationFile.LocaleNameEN);
            WriteString(stream, localizationFile.LocaleNameID);
            WriteString(stream, localizationFile.LocaleNameLocalized);
            WriteLocalizationFilesLocalizations(localizationFile, stream);
            WriteLocalizationFilesIndices(localizationFile, stream);
            workAround.Stop(stream);
            stream.Flush();
        });
    }

    private static void WriteLocalizationFilesIndices(ILocalizationFile localizationFile, Stream stream) {
        WriteInt32(stream, localizationFile.Indices.Count);
        foreach (KeyValuePair<string, int> entry in localizationFile.Indices) {
            WriteString(stream, entry.Key);
            WriteInt32(stream, entry.Value);
        }
    }

    private static void WriteLocalizationFilesLocalizations(ILocalizationFile localizationFile, Stream stream) {
        WriteInt32(stream, localizationFile.Localizations.Count);
        foreach (ILocalizationEntry entry in localizationFile.Localizations) {
            WriteString(stream, entry.Key);
            if (StringHelper.IsNullOrWhiteSpaceOrEmpty(entry.Translation)) {
                if (StringHelper.IsNullOrWhiteSpaceOrEmpty(entry.ValueMerge)) {
                    WriteString(stream, entry.Value);
                } else {
                    WriteString(stream, entry.ValueMerge);
                }
            } else {
                WriteString(stream, entry.Translation);
            }
        }
    }

    private static short ReadInt16(Stream stream) {
        byte[] buffer = new byte[2];
        stream.ReadExactly(buffer);
        return BinaryPrimitives.ReadInt16LittleEndian(buffer);
    }
    private static void WriteInt16(Stream stream, short val) {
        byte[] buffer = new byte[2];
        BinaryPrimitives.WriteInt16LittleEndian(buffer, val);
        stream.Write(buffer);
    }
    private static int ReadInt32(Stream stream) {
        byte[] buffer = new byte[4];
        stream.ReadExactly(buffer);
        return BinaryPrimitives.ReadInt32LittleEndian(buffer);
    }
    private static void WriteInt32(Stream stream, int val) {
        byte[] buffer = new byte[4];
        BinaryPrimitives.WriteInt32LittleEndian(buffer, val);
        stream.Write(buffer);
    }
    private static string ReadString(Stream stream) {
        uint len = LittleEndianBase128.DecodeUnsignedInteger(stream);
        byte[] buffer = new byte[len];
        stream.ReadExactly(buffer);
        return Encoding.UTF8.GetString(buffer);
    }
    private static void WriteString(Stream stream, string val) {
        byte[] valueBytes = Encoding.UTF8.GetBytes(val);
        byte[] lenBytes = LittleEndianBase128.EncodeUnsignedInteger((uint) valueBytes.Length);
        stream.Write(lenBytes);
        stream.Write(valueBytes);
    }


    private FileInfo GetLocalizationFileInfo(string? fileName) {
        return this.GetLocalizationFiles().Where(item => item.Name == fileName).First();
    }


    /// <summary>
    ///     workaround - "pl-PL.loc", "zh-HANS.loc" and "zh-HANT.loc" have more content after indices
    /// </summary>
    private class WorkAround {
        private byte[] _extraContent = Array.Empty<byte>();
        private readonly bool _skip;
        public WorkAround(bool skip) {
            this._skip = skip;
        }
        public void Start(FileInfo fileInfo) {
            if (this._skip) {
                return;
            }
            //
            // read loc file with this apps logic
            using Stream workaround = File.OpenRead(fileInfo.FullName);
            GetLocalizationFile(fileInfo, workaround);
            //
            // copy remaining bytes (extra content)
            using MemoryStream memoryStream = new MemoryStream();
            workaround.CopyTo(memoryStream);
            this._extraContent = memoryStream.ToArray();
        }
        public void Stop(Stream stream) {
            if (this._skip) {
                return;
            }
            stream.Write(this._extraContent);
        }
    }
}
