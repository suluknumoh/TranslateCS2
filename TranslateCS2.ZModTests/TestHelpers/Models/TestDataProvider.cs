using System;
using System.IO;

using TranslateCS2.Inf;

namespace TranslateCS2.ZModTests.TestHelpers.Models;
public class TestDataProvider : IDisposable {
    private int randomCounter = 0;
    private bool generated;
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
    }
    public void GenerateData(bool overwriteExisting = false,
                             bool addKey = false) {
        if (this.generated
            && !overwriteExisting) {
            return;
        }
        this.randomCounter++;
        JSONGenerator generator = new JSONGenerator(this.DataDirectorySpecific, this.randomCounter, addKey);
        generator.Generate(true, true);
        this.EntryCountPerFile = generator.EntryCountPerFile;
        this.generated = true;
    }
    public void GenerateCorruptData(bool overwriteExisting = false,
                                    bool addKey = false) {
        if (this.generated
            && !overwriteExisting) {
            return;
        }
        this.randomCounter++;
        JSONGenerator generator = new JSONGenerator(this.DataDirectorySpecific, this.randomCounter, addKey);
        generator.Generate(false, true);
        this.EntryCountPerFile = generator.EntryCountPerFile;
        this.generated = true;
    }
    public void Dispose() {
        if (Directory.Exists(this.DirectoryName)) {
            Directory.Delete(this.DirectoryName, true);
        }
    }
}
