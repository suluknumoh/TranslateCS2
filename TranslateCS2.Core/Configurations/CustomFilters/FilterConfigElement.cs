using System.Configuration;

namespace TranslateCS2.Core.Configurations.CustomFilters;
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.configuration.configurationelementcollection?view=dotnet-plat-ext-8.0"/>>
public class FilterConfigElement : ConfigurationElement {
    public FilterConfigElement(string name, string values, string checkmethod) {
        this.Name = name;
        this.Values = values;
        this.CheckMethod = checkmethod;
    }

    public FilterConfigElement() {
    }

    [ConfigurationProperty("name", IsRequired = true, IsKey = true)]
    public string Name {
        get => (string) this["name"];
        set => this["name"] = value;
    }

    [ConfigurationProperty("values", IsRequired = true)]
    public string Values {
        get => (string) this["values"];
        set => this["values"] = value;
    }

    [ConfigurationProperty("checkmethod", IsRequired = true)]
    public string CheckMethod {
        get => (string) this["checkmethod"];
        set => this["checkmethod"] = value;
    }
}
