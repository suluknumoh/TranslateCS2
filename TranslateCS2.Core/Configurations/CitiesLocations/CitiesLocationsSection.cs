using System;
using System.Collections;
using System.Configuration;

using TranslateCS2.Inf;

namespace TranslateCS2.Core.Configurations.CitiesLocations;
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.configuration.configurationelementcollection?view=dotnet-plat-ext-8.0"/>>
public class CitiesLocationsSection : ConfigurationSection {
    private static string SectionName { get; } = "CitiesLocations";
    private const string PropertyName = "Locations";
    private const string AddItemName = "Location";
    [ConfigurationProperty(PropertyName, IsDefaultCollection = false)]
    [ConfigurationCollection(typeof(CitiesLocations), AddItemName = AddItemName)]
    public CitiesLocations Locations {
        get {
            CitiesLocations? locations = base[PropertyName] as CitiesLocations;
            if (locations is null) {
                locations = [];
                base[PropertyName] = locations;
            }
            return locations;
        }
    }
    public CitiesLocationsSection() { }

    public static CitiesLocationsSection? GetReadOnly() {
        return ConfigurationManager.GetSection(CitiesLocationsSection.SectionName) as CitiesLocationsSection;
    }
    public static void AddLocation(string location) {
        Configuration config = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
        config.TypeStringTransformer += ConvertTypeString;
        //
        IEnumerator enumerator = config.Sections.GetEnumerator();
        CitiesLocationsSection? section = null;
        while (enumerator.MoveNext()) {
            object current = enumerator.Current;
            if (current.GetType().IsAssignableTo(typeof(CitiesLocationsSection))) {
                section = (CitiesLocationsSection) current;
            }
        }
        if (section is null) {
            section = new CitiesLocationsSection();
            config.Sections.Add(CitiesLocationsSection.SectionName, section);
        }
        CitiesLocation citiesLocation = new CitiesLocation {
            Path = location
        };
        section.Locations.Add(citiesLocation);
        config.Save(ConfigurationSaveMode.Minimal);
        ConfigurationManager.RefreshSection(CitiesLocationsSection.SectionName);
    }

    /// <summary>
    ///     to remove Version, Culture and Token from Type-String
    /// </summary>
    private static string ConvertTypeString(string typeString) {
        string separator = StringConstants.CommaSpace;
        string[] source = typeString.Split(separator);
        string[] destination = new string[2];
        Array.Copy(source, destination, 2);
        return String.Join(separator, destination);
    }
}
