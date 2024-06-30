using System;
using System.Collections.Generic;
using System.Text;

using TranslateCS2.Inf.Attributes;
using TranslateCS2.Inf.Models.Localizations;
using TranslateCS2.Mod.Enums;

namespace TranslateCS2.Mod.Containers.Items;
internal class FlavorSource : IEquatable<FlavorSource?> {
    public FlavorSourceTypes SourceType { get; }
    public MyLocalization<string> Localization { get; }
    public MyLocalizationSource<string> Source => this.Localization.Source;
    public int EntryCount => this.Localization.EntryCount;
    public bool IsOk => this.Localization.IsOK && !this.HasErrors;
    public bool HasErrors { get; set; }
    public string ModName { get; }
    public string ModId { get; }
    public FlavorSource(FlavorSourceTypes sourceType,
                        MyLocalization<string> localization,
                        string modName,
                        string modId) {
        this.SourceType = sourceType;
        this.Localization = localization;
        this.ModName = modName;
        this.ModId = modId;
    }



    [MyExcludeFromCoverage]
    public override string ToString() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(this.GetType().ToString());
        builder.AppendLine($"{nameof(this.ModId)}: {this.ModId}");
        builder.AppendLine($"{nameof(this.ModName)}: {this.ModName}");
        builder.AppendLine($"{nameof(this.SourceType)}: {this.SourceType}");
        builder.AppendLine($"{nameof(this.Localization)}: {this.Localization}");
        builder.AppendLine($"{nameof(this.IsOk)}: {this.IsOk}");
        builder.AppendLine($"{nameof(this.HasErrors)}: {this.HasErrors}");
        return builder.ToString();
    }

    [MyExcludeFromCoverage]
    public override bool Equals(object? obj) {
        return this.Equals(obj as FlavorSource);
    }

    [MyExcludeFromCoverage]
    public bool Equals(FlavorSource? other) {
        return other is not null &&
               this.SourceType == other.SourceType &&
               EqualityComparer<MyLocalization<string>>.Default.Equals(this.Localization, other.Localization) &&
               this.EntryCount == other.EntryCount &&
               this.IsOk == other.IsOk &&
               this.HasErrors == other.HasErrors &&
               this.ModName == other.ModName &&
               this.ModId == other.ModId;
    }

    [MyExcludeFromCoverage]
    public override int GetHashCode() {
        int hashCode = -221263353;
        hashCode = (hashCode * -1521134295) + this.SourceType.GetHashCode();
        hashCode = (hashCode * -1521134295) + EqualityComparer<MyLocalization<string>>.Default.GetHashCode(this.Localization);
        hashCode = (hashCode * -1521134295) + this.EntryCount.GetHashCode();
        hashCode = (hashCode * -1521134295) + this.IsOk.GetHashCode();
        hashCode = (hashCode * -1521134295) + this.HasErrors.GetHashCode();
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.ModName);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.ModId);
        return hashCode;
    }
}
