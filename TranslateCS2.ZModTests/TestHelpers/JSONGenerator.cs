using System.IO;
using System.Text;

using TranslateCS2.Inf;

namespace TranslateCS2.ZZZModTestLib.TestHelpers;
internal class JSONGenerator {
    public string Destination;
    public int EntryCountPerFile { get; private set; }
    public JSONGenerator(string destination) {
        this.Destination = destination;
    }
    /// <summary>
    ///     generates localization JSONs in the given <see cref="Destination"/>
    /// </summary>
    /// <param name="ok">
    ///     <see langword="true"/> (<see langword="default"/>) - generates NON corrupt data
    ///     <br/>
    ///     <see langword="false"/> (has to be set explicitly) - generates corrupt data
    /// </param>
    /// <param name="overwrite">
    ///     <see langword="true"/> (<see langword="default"/>) - overwrites existing data
    ///     <br/>
    ///     <see langword="false"/> (has to be set explicitly) - does not overwrite existing data
    /// </param>
    public void Generate(bool ok = true,
                         bool overwrite = true) {
        Directory.CreateDirectory(this.Destination);
        foreach (string supportedLocale in LocalesSupported.Lowered) {
            string filePath = Path.Combine(this.Destination, $"{supportedLocale}{ModConstants.JsonExtension}");
            if (File.Exists(filePath)
                && !overwrite) {
                continue;
            }
            this.EntryCountPerFile = 0;
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("{");
            // to see, if changes are applied on startup
            this.AppendEntry(builder,
                             ok,
                             "Menu.OPTIONS",
                             $"Options ({supportedLocale}.json)");
            // to see, if changes are applied on flavor changed
            this.AppendEntry(builder,
                             ok,
                             "Options.SECTION[General]",
                             $"General ({supportedLocale}.json)");
            if (false) {
                // just a template...
                this.AppendEntry(builder,
                                 ok,
                                 "key",
                                 $"value");
            }
            builder.AppendLine("}");
            File.WriteAllText(filePath,
                              builder.ToString(),
                              Encoding.UTF8);

        }
    }
    private void AppendEntry(StringBuilder builder,
                             bool ok,
                             string key,
                             string value) {
        string colon = "";
        if (ok) {
            colon = ":";
        }
        builder.AppendLine($"\"{key}\"{colon} \"{value}\",");
        this.EntryCountPerFile++;
    }
}
