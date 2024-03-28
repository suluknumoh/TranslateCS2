using System;
using System.Buffers.Binary;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

using TranslateCS2.Helpers;
using TranslateCS2.Models;
using TranslateCS2.Models.LocDictionary;

namespace TranslateCS2.Services;
internal class LocalizationFilesService {
    private readonly InstallPathDetector _installPathDetector;

    public LocalizationFilesService(InstallPathDetector installPathDetector) {
        this._installPathDetector = installPathDetector;
    }
    public IEnumerable<FileInfo> GetLocalizationFiles() {
        string installPath = this._installPathDetector.DetectInstallPath();
        string locLocation = Path.Combine(installPath, "Cities2_Data", "StreamingAssets", "Data~");
        DirectoryInfo loc = new DirectoryInfo(locLocation);
        return loc.EnumerateFiles("*.loc");
    }
    /// <seealso href="https://github.com/grotaclas/PyHelpersForPDXWikis/blob/main/cs2/localization.py">
    public LocalizationFile GetLocalizationFile(FileInfo fileInfo) {
        using Stream stream = File.OpenRead(fileInfo.FullName);
        short fileHeader = ReadInt16(stream);
        string localeNameEN = ReadString(stream);
        string localeNameID = ReadString(stream);
        string localeNameLocalized = ReadString(stream);
        LocalizationFile localizationFile = new LocalizationFile(fileInfo.Name, fileHeader, localeNameEN, localeNameID, localeNameLocalized);

        int localizationCount = ReadInt32(stream);
        for (int i = 0; i < localizationCount; i++) {
            string key = ReadString(stream);
            string value = ReadString(stream);
            LocalizationDictionaryEntry originLocalizationKey = new LocalizationDictionaryEntry(key, value, null);
            localizationFile.LocalizationDictionary.Add(originLocalizationKey);
        }

        int indexCount = ReadInt32(stream);
        for (int i = 0; i < indexCount; i++) {
            string key = ReadString(stream);
            int val = ReadInt32(stream);
            localizationFile.Indizes.Add(new KeyValuePair<string, int>(key, val));
        }
        return localizationFile;
    }
    public void WriteLocalizationFileDirect(LocalizationFile localizationFile) {
        this.WriteLocalizationFileDirect(localizationFile, null);
    }
    /// <summary>
    ///     writes the given <see cref="LocalizationFile{T}"/>
    /// </summary>
    /// <param name="localizationFile">
    ///     the <see cref="LocalizationFile{T}"/> to write
    /// </param>
    /// <param name="fileInfo">
    ///     currently for testing-purposes only
    /// </param>
    public void WriteLocalizationFileDirect(LocalizationFile localizationFile, FileInfo? fileInfo) {
        fileInfo ??= this.GetLocalizationFileInfo(localizationFile.FileName);
        fileInfo.Delete();
        using Stream stream = File.OpenWrite(fileInfo.FullName);
        WriteInt16(stream, localizationFile.FileHeader);
        WriteString(stream, localizationFile.LocaleNameEN);
        WriteString(stream, localizationFile.LocaleNameID);
        WriteString(stream, localizationFile.LocaleNameLocalized);
        WriteInt32(stream, localizationFile.LocalizationDictionary.Count);
        foreach (LocalizationDictionaryEntry entry in localizationFile.LocalizationDictionary) {
            WriteString(stream, entry.Key);
            if (String.IsNullOrEmpty(entry.Translation) || String.IsNullOrWhiteSpace(entry.Translation)) {
                WriteString(stream, entry.ValueMerge);
            } else {
                WriteString(stream, entry.Translation);
            }
        }
        WriteInt32(stream, localizationFile.Indizes.Count);
        foreach (KeyValuePair<string, int> entry in localizationFile.Indizes) {
            WriteString(stream, entry.Key);
            WriteInt32(stream, entry.Value);
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
}
