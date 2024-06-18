using System;
using System.Collections.Generic;
using System.Text;

using TranslateCS2.Inf.Attributes;

namespace TranslateCS2.Inf.Models.Localizations;
public class MyLocalization<E> : IEquatable<MyLocalization<E>?> {
    private readonly string uniquer = ModConstants.Name;
    public string Id { get; }
    public virtual string Name { get; }
    public string NameEnglish { get; }
    public bool IsOK => this.EntryCount > 0;
    public int EntryCount => this.Source.Localizations?.Count ?? 0;
    public virtual MyLocalizationSource<E> Source { get; }
    public MyLocalization(string id,
                          string nameEnglish,
                          string name,
                          MyLocalizationSource<E> source) {
        this.Id = id;
        this.NameEnglish = nameEnglish;
        this.Name = name;
        this.Source = source;
    }

    [MyExcludeFromCoverage]
    public override string ToString() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(this.GetType().ToString());
        builder.AppendLine($"{nameof(this.Id)}: {this.Id}");
        builder.AppendLine($"{nameof(this.NameEnglish)}: {this.NameEnglish}");
        builder.AppendLine($"{nameof(this.Name)}: {this.Name}");
        builder.AppendLine($"{nameof(this.IsOK)}: {this.IsOK}");
        builder.AppendLine($"{nameof(this.EntryCount)}: {this.EntryCount}");
        builder.Append($"{nameof(this.Source)}: {this.Source}");
        return builder.ToString();
    }

    [MyExcludeFromCoverage]
    public override bool Equals(object? obj) {
        return this.Equals(obj as MyLocalization<E>);
    }

    [MyExcludeFromCoverage]
    public bool Equals(MyLocalization<E>? other) {
        return other is not null &&
               this.uniquer == other.uniquer &&
               this.Id == other.Id &&
               this.Name == other.Name &&
               this.NameEnglish == other.NameEnglish;
    }

    [MyExcludeFromCoverage]
    public override int GetHashCode() {
        int hashCode = -329714928;
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.uniquer);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.Id);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.Name);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.NameEnglish);
        return hashCode;
    }
}
