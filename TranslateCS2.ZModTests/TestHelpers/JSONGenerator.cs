using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Text;

using TranslateCS2.Inf;

namespace TranslateCS2.ZModTests.TestHelpers;
internal class JSONGenerator {
    private readonly int randomCounter = 0;
    private bool addRandom => this.randomCounter > 0;
    public string Destination;
    public int EntryCountPerFile { get; private set; }
    public JSONGenerator(string destination,
                         int randomCounter = 0) {
        this.Destination = destination;
        this.randomCounter = randomCounter;
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
        List<CultureInfo> supported = CultureInfoHelper.GetSupportedCultures();
        foreach (CultureInfo supportedLocale in supported) {
            string filePath = Path.Combine(this.Destination, $"{supportedLocale.Name.ToLower()}{ModConstants.JsonExtension}");
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
            if (this.addRandom) {
                this.AppendEntry(builder,
                                 ok,
                                 "This.Is.Just.A.Random.Key",
                                 this.randomCounter.ToString("D0"));
            }
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
