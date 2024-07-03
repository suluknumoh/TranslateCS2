using System;
using System.Collections.Generic;
using System.Text;

using TranslateCS2.Inf.Attributes;
using TranslateCS2.Mod.Enums;

namespace TranslateCS2.Mod.Models;
internal class FlavorSourceInfo : IEquatable<FlavorSourceInfo?> {
    public int Id { get; }
    public string Name { get; }
    public Version Version { get; }
    public bool IsLocal { get; }
    public FlavorSourceTypes FlavorSourceType { get; }
    public FlavorSourceInfo(int id,
                            string name,
                            Version version,
                            bool isLocal,
                            FlavorSourceTypes flavorSourceType) {
        this.Id = id;
        this.Name = name;
        this.Version = version;
        this.IsLocal = isLocal;
        this.FlavorSourceType = flavorSourceType;
    }

    [MyExcludeFromCoverage]
    public override bool Equals(object? obj) {
        return this.Equals(obj as FlavorSourceInfo);
    }

    [MyExcludeFromCoverage]
    public bool Equals(FlavorSourceInfo? other) {
        return other is not null &&
               this.Id == other.Id &&
               this.Name == other.Name &&
               EqualityComparer<Version>.Default.Equals(this.Version, other.Version) &&
               this.IsLocal == other.IsLocal &&
               this.FlavorSourceType == other.FlavorSourceType;
    }

    [MyExcludeFromCoverage]
    public override int GetHashCode() {
        int hashCode = -1597829199;
        hashCode = (hashCode * -1521134295) + this.Id.GetHashCode();
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.Name);
        hashCode = (hashCode * -1521134295) + EqualityComparer<Version>.Default.GetHashCode(this.Version);
        hashCode = (hashCode * -1521134295) + this.IsLocal.GetHashCode();
        hashCode = (hashCode * -1521134295) + this.FlavorSourceType.GetHashCode();
        return hashCode;
    }

    [MyExcludeFromCoverage]
    public override string ToString() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(nameof(FlavorSourceInfo));
        builder.AppendLine($"{nameof(this.Id)}: {this.Id}");
        builder.AppendLine($"{nameof(this.Name)}: {this.Name}");
        builder.AppendLine($"{nameof(this.Version)}: {this.Version}");
        builder.AppendLine($"{nameof(this.IsLocal)}: {this.IsLocal}");
        builder.AppendLine($"{nameof(this.FlavorSourceType)}: {this.FlavorSourceType}");
        return builder.ToString();
    }
}
