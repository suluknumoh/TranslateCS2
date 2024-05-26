using System;
using System.IO;

using TranslateCS2.Inf;

namespace TranslateCS2.ZZZModTestLib;
public class TestDataProvider : IDisposable {

    public string DirectoryName { get; set; }
    private string DataDirectoryGeneral => $"{this.DirectoryName}{StringConstants.ForwardSlash}{ModConstants.ModsData}";
    private string DataDirectorySpecific => $"{this.DataDirectoryGeneral}{StringConstants.ForwardSlash}{ModConstants.Name}{StringConstants.ForwardSlash}";
    public TestDataProvider() {
        this.DirectoryName = nameof(TestDataProvider);
        this.GenerateData();
    }
    public void GenerateData() {
        Directory.CreateDirectory(this.DataDirectorySpecific);
        JSONGenerator.Generate(this.DataDirectorySpecific, true, true);
    }
    public void GenerateCorruptData() {
        Directory.CreateDirectory(this.DataDirectorySpecific);
        JSONGenerator.Generate(this.DataDirectorySpecific, false, true);
    }
    public void Dispose() {
        if (Directory.Exists(this.DirectoryName)) {
            Directory.Delete(this.DirectoryName, true);
        }
    }
}
