using System.Collections.Generic;
using System.Globalization;
using System.Linq;

using TranslateCS2.Mod.Helpers;

using UnityEngine;

namespace TranslateCS2.Mod.Models;
internal class MyCountry {
    public string ID { get; private set; }
    public string Name { get; private set; }
    public IList<TranslationFile> Flavors { get; } = [];
    public IList<CultureInfo> CultureInfos { get; } = [];
    public SystemLanguage SystemLanguage { get; }
    public bool IsBuiltIn { get; private set; }
    public bool HasFlavors => this.Flavors.Any();
    public MyCountry(SystemLanguage systemLanguage) {
        this.SystemLanguage = systemLanguage;
    }
    public void Init() {
        IEnumerable<CultureInfo> builtin = this.CultureInfos.Where(ci => LocaleHelper.BuiltIn.Contains(ci.Name));
        if (builtin.Any()) {
            CultureInfo ci = builtin.First();
            this.ID = ci.Name;
            this.Name = ci.EnglishName;
            this.IsBuiltIn = true;
        } else {
            IEnumerable<CultureInfo> remaining = this.CultureInfos.Where(ci => !ci.Name.Contains("-"));
            if (remaining.Any()) {
                CultureInfo ci = remaining.First();
                this.ID = ci.Name;
                this.Name = ci.EnglishName;
                this.IsBuiltIn = false;
            }
        }
    }
}
