using System.Configuration;

namespace TranslateCS2.Core.Configurations.CustomFilters;
/// <seealso href="https://learn.microsoft.com/en-us/dotnet/api/system.configuration.configurationelementcollection?view=dotnet-plat-ext-8.0"/>>
public class FiltersCollection : ConfigurationElementCollection {
    public FiltersCollection() { }
    public override ConfigurationElementCollectionType CollectionType => ConfigurationElementCollectionType.AddRemoveClearMap;
    protected override ConfigurationElement CreateNewElement() {
        return new FilterConfigElement();
    }
    protected override object GetElementKey(ConfigurationElement element) {
        return ((FilterConfigElement) element).Name;
    }
    public FilterConfigElement this[int index] {
        get => (FilterConfigElement) this.BaseGet(index);
        set {
            if (this.BaseGet(index) != null) {
                this.BaseRemoveAt(index);
            }
            this.BaseAdd(index, value);
        }
    }
    public new FilterConfigElement this[string Name] => (FilterConfigElement) this.BaseGet(Name);
    public int IndexOf(FilterConfigElement url) {
        return this.BaseIndexOf(url);
    }
    public void Add(FilterConfigElement url) {
        this.BaseAdd(url);
    }
    protected override void BaseAdd(ConfigurationElement element) {
        this.BaseAdd(element, false);
    }
    public void Remove(FilterConfigElement url) {
        if (this.BaseIndexOf(url) >= 0) {
            this.BaseRemove(url.Name);
        }
    }
    public void RemoveAt(int index) {
        this.BaseRemoveAt(index);
    }
    public void Remove(string name) {
        this.BaseRemove(name);
    }
    public void Clear() {
        this.BaseClear();
    }
}