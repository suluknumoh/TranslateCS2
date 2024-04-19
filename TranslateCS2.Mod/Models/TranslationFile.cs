using System.IO;
using System.Text;

namespace TranslateCS2.Mod.Models;
internal class TranslationFile {
    public string Name { get; }
    public string Path { get; }
    public TranslationFile(string name, string path) {
        this.Name = name;
        this.Path = path;
    }
    public override string ToString() {
        return $"{nameof(this.Name)}: {this.Name} - {nameof(this.Path)}: {this.Path}";
    }
    public string ReadJson() {
        return File.ReadAllText(this.Path, Encoding.UTF8);
    }
}
