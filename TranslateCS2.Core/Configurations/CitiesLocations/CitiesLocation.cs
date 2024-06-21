using System.Configuration;

namespace TranslateCS2.Core.Configurations.CitiesLocations;
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.configuration.configurationelementcollection?view=dotnet-plat-ext-8.0"/>>
public class CitiesLocation : ConfigurationElement {
    private const string PathProperty = "path";
    public CitiesLocation(string path) {
        this.Path = path;
    }

    public CitiesLocation() { }

    [ConfigurationProperty(PathProperty, IsRequired = true, IsKey = true)]
    public string Path {
        get => (string) this[PathProperty];
        set => this[PathProperty] = value;
    }
}
