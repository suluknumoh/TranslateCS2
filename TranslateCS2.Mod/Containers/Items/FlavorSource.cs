using System;
using System.Collections.Generic;
using System.Text;

using TranslateCS2.Inf.Attributes;
using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Mod.Models;

namespace TranslateCS2.Mod.Containers.Items;
internal class FlavorSource : IEquatable<FlavorSource?> {
    public FlavorSourceInfo FlavorSourceInfo { get; }
    public MyLocalization<string> Localization { get; }
    public MyLocalizationSource<string> Source => this.Localization.Source;
    public int EntryCount => this.Localization.EntryCount;
    public bool IsOk => this.Localization.IsOK && !this.HasErrors;
    public bool HasErrors { get; set; }
    public FlavorSource(FlavorSourceInfo flavorSourceInfo,
                        MyLocalization<string> localization) {
        this.FlavorSourceInfo = flavorSourceInfo;
        this.Localization = localization;
    }



    [MyExcludeFromCoverage]
    public override string ToString() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(this.GetType().ToString());
        builder.AppendLine($"{nameof(this.FlavorSourceInfo)}: {this.FlavorSourceInfo}");
        builder.AppendLine($"{nameof(this.IsOk)}: {this.IsOk}");
        builder.AppendLine($"{nameof(this.HasErrors)}: {this.HasErrors}");
        builder.AppendLine($"{nameof(this.Localization)}: {this.Localization}");
        return builder.ToString();
    }

    [MyExcludeFromCoverage]
    public override bool Equals(object? obj) {
        return this.Equals(obj as FlavorSource);
    }

    [MyExcludeFromCoverage]
    public bool Equals(FlavorSource? other) {
        return other is not null &&
               EqualityComparer<FlavorSourceInfo>.Default.Equals(this.FlavorSourceInfo, other.FlavorSourceInfo) &&
               EqualityComparer<MyLocalization<string>>.Default.Equals(this.Localization, other.Localization);
    }

    [MyExcludeFromCoverage]
    public override int GetHashCode() {
        int hashCode = -2049087224;
        hashCode = (hashCode * -1521134295) + EqualityComparer<FlavorSourceInfo>.Default.GetHashCode(this.FlavorSourceInfo);
        hashCode = (hashCode * -1521134295) + EqualityComparer<MyLocalization<string>>.Default.GetHashCode(this.Localization);
        return hashCode;
    }
}
