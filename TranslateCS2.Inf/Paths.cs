using System;
using System.IO;

namespace TranslateCS2.Inf;
/// <seealso cref="https://cs2.paradoxwikis.com/Naming_Folder_And_Files"/>
public class Paths {
    private const string forwardSlash = "/";
    private const string backSlash = "\\";
    public string UserDataPath { get; }
    public string ModsDataPathGeneral { get; }
    public string ModsDataPathSpecific { get; }
    public string StreamingDataPath { get; }
    public Paths(bool createIfNotExists, string streamingDataPath, string? userDataPath = null) {
        // paths have to end with a forwardslash!
        this.StreamingDataPath = streamingDataPath;
        if (!this.StreamingDataPath.EndsWith(forwardSlash)) {
            this.StreamingDataPath += forwardSlash;
        }
        this.UserDataPath = userDataPath ?? $"{Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData).Replace("\\", forwardSlash)}Low/Colossal Order/Cities Skylines II";
        this.UserDataPath += forwardSlash;
        this.ModsDataPathGeneral = $"{this.UserDataPath}{ModConstants.ModsData}{forwardSlash}";
        this.ModsDataPathSpecific = $"{this.ModsDataPathGeneral}{ModConstants.Name}{forwardSlash}";
        if (createIfNotExists) {
            this.CreateIfNotExists();
        }
    }
    private void CreateIfNotExists() {
        Directory.CreateDirectory(this.ModsDataPathSpecific);
    }
    public string? TryToGetModsPath() {
        if (Directory.Exists(this.ModsDataPathGeneral)) {
            if (Directory.Exists(this.ModsDataPathSpecific)) {
                return this.ModsDataPathSpecific;
            }
            return this.ModsDataPathGeneral;
        }
        return null;
    }
    public string ExtractLocaleIdFromPath(string path) {
        return
            path
                .Replace(this.ModsDataPathSpecific, String.Empty)
                .Replace(ModConstants.JsonExtension, String.Empty);
    }
    public static string? NormalizeUnix(string path) {
        if (path == null) {
            return path;
        }
        return path.Replace(backSlash, forwardSlash);
    }
    public static string? NormalizeWindows(string? path) {
        if (path == null) {
            return path;
        }
        return path.Replace(forwardSlash, backSlash);
    }
}
