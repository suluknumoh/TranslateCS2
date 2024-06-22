using System.Configuration;

namespace TranslateCS2.Core.Configurations.CitiesLocations;
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.configuration.configurationelementcollection?view=dotnet-plat-ext-8.0"/>>
public class CitiesLocations : ConfigurationElementCollection {
    public CitiesLocations() { }
    public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.AddRemoveClearMap;
    protected override ConfigurationElement CreateNewElement() {
        return new CitiesLocation();
    }
    protected override object GetElementKey(ConfigurationElement element) {
        return ((CitiesLocation) element).Path;
    }
    public CitiesLocation this[int index] {
        get => (CitiesLocation) this.BaseGet(index);
        set {
            if (this.BaseGet(index) is not null) {
                this.BaseRemoveAt(index);
            }
            this.BaseAdd(index, value);
        }
    }
    public new CitiesLocation this[string path] => (CitiesLocation) this.BaseGet(path);
    public int IndexOf(CitiesLocation element) {
        return this.BaseIndexOf(element);
    }
    public void Add(CitiesLocation element) {
        this.BaseAdd(element);
    }
    protected override void BaseAdd(ConfigurationElement element) {
        this.BaseAdd(element, false);
    }
    public void Remove(CitiesLocation element) {
        if (this.BaseIndexOf(element) >= 0) {
            this.BaseRemove(element.Path);
        }
    }
    public void RemoveAt(int index) {
        this.BaseRemoveAt(index);
    }
    public void Remove(string path) {
        this.BaseRemove(path);
    }
    public void Clear() {
        this.BaseClear();
    }
}
