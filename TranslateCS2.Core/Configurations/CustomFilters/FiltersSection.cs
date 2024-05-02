using System.Configuration;

namespace TranslateCS2.Core.Configurations.CustomFilters;
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.configuration.configurationelementcollection?view=dotnet-plat-ext-8.0"/>>
public class FiltersSection : ConfigurationSection {
    [ConfigurationProperty("filters", IsDefaultCollection = false)]
    [ConfigurationCollection(typeof(FiltersCollection), AddItemName = "add")]
    public FiltersCollection Filters {
        get {
            FiltersCollection filtersCollection = (FiltersCollection) base["filters"];

            return filtersCollection;
        }
    }
    public FiltersSection() { }
}
