using System;
using System.IO;

using TranslateCS2.Inf;

namespace TranslateCS2.ZZZModTestLib;
public class TestDataProvider : IDisposable {
    private string _DirectoryName;
    public string DirectoryName {
        get => this._DirectoryName;
        set {
            this._DirectoryName = value;
            if (!this._DirectoryName.EndsWith(StringConstants.ForwardSlash)) {
                this._DirectoryName += StringConstants.ForwardSlash;
            }
        }
    }
    private string DataDirectoryGeneral => $"{this.DirectoryName}{ModConstants.DataPathRawGeneral}";
    private string DataDirectorySpecific => $"{this.DirectoryName}{ModConstants.DataPathRawSpecific}";
    public int EntryCountPerFile { get; private set; }
    public TestDataProvider() {
        this.DirectoryName = nameof(TestDataProvider);
        this.GenerateData();
    }
    public void GenerateData() {
        this.EntryCountPerFile = JSONGenerator.Generate(this.DataDirectorySpecific, true, true);
    }
    public void GenerateCorruptData() {
        this.EntryCountPerFile = JSONGenerator.Generate(this.DataDirectorySpecific, false, true);
    }
    public void Dispose() {
        if (Directory.Exists(this.DirectoryName)) {
            Directory.Delete(this.DirectoryName, true);
        }
    }
}
