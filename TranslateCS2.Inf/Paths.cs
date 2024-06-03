using System;
using System.IO;
using System.Text;

namespace TranslateCS2.Inf;
/// <seealso cref="https://cs2.paradoxwikis.com/Naming_Folder_And_Files"/>
public class Paths {
    public string UserDataPath { get; }
    public string ModsDataPathGeneral { get; }
    public string ModsDataPathSpecific { get; }
    public string StreamingDataPath { get; }
    public string StreamingDatasDataPath { get; }
    public Paths(bool createIfNotExists,
                 string streamingDataPath,
                 string? userDataPath = null) {
        // paths have to end with a forwardslash!
        if (streamingDataPath is null) {
            throw new ArgumentNullException(nameof(streamingDataPath));
        }
        this.StreamingDataPath = AppendForwardSlashIfNecessary(streamingDataPath);
        this.StreamingDatasDataPath = this.StreamingDataPath;
        this.StreamingDatasDataPath += StringConstants.DataTilde;
        this.StreamingDatasDataPath += StringConstants.ForwardSlash;
        this.UserDataPath = userDataPath ?? GetFallbackUserDataPathUnixFormat();
        this.UserDataPath = AppendForwardSlashIfNecessary(this.UserDataPath);
        this.ModsDataPathGeneral = $"{this.UserDataPath}{ModConstants.DataPathRawGeneral}";
        this.ModsDataPathSpecific = $"{this.UserDataPath}{ModConstants.DataPathRawSpecific}";
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
    public static string? NormalizeUnix(string? path) {
        if (path is null) {
            return path;
        }
        return path.Replace(StringConstants.BackSlash, StringConstants.ForwardSlash);
    }
    public static string? NormalizeWindows(string? path) {
        if (path is null) {
            return path;
        }
        return path.Replace(StringConstants.ForwardSlash, StringConstants.BackSlash);
    }
    public static string GetFallbackUserDataPathUnixFormat() {
        StringBuilder builder = new StringBuilder();
        builder.Append(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData));
        builder.Append(StringConstants.Low);
        builder.Append(StringConstants.ForwardSlash);
        builder.Append(StringConstants.Colossal_Order);
        builder.Append(StringConstants.ForwardSlash);
        builder.Append(StringConstants.Cities_Skylines_II);
        return NormalizeUnix(builder.ToString()) ?? builder.ToString();
    }

    public static string AppendForwardSlashIfNecessary(string s) {
        if (s.EndsWith(StringConstants.ForwardSlash)) {
            return s;
        }
        return s + StringConstants.ForwardSlash;
    }
}
