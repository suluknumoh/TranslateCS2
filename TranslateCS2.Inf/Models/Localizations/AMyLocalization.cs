using System;
using System.Collections.Generic;

namespace TranslateCS2.Inf.Models.Localizations;
public abstract class AMyLocalization<S, L, E> : IEquatable<AMyLocalization<S, L, E>?> where S : IMyLocalizationSource<L, E>
                                                                                       where L : ICollection<E>
                                                                                       where E : MyLocalizationEntry {
    private string uniquer { get; } = ModConstants.Name;
    public string Id { get; private set; }
    public string Name { get; private set; }
    public string NameEnglish { get; private set; }
    public bool IsOK => this.EntryCount > 0;
    public int EntryCount => this.Source.Localizations?.Count ?? 0;
    public S Source { get; }
    public AMyLocalization(string id,
                           string nameEnglish,
                           string name,
                           S source) {
        this.Id = id;
        this.NameEnglish = nameEnglish;
        this.Name = name;
        this.Source = source;
    }

    public override bool Equals(object? obj) {
        return this.Equals(obj as AMyLocalization<S, L, E>);
    }

    public bool Equals(AMyLocalization<S, L, E>? other) {
        return other is not null &&
               this.uniquer == other.uniquer &&
               this.Id == other.Id &&
               this.Name == other.Name &&
               this.NameEnglish == other.NameEnglish;
    }

    public override int GetHashCode() {
        int hashCode = -329714928;
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.uniquer);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.Id);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.Name);
        hashCode = (hashCode * -1521134295) + EqualityComparer<string>.Default.GetHashCode(this.NameEnglish);
        return hashCode;
    }

    public static bool operator ==(AMyLocalization<S, L, E>? left, AMyLocalization<S, L, E>? right) {
        return EqualityComparer<AMyLocalization<S, L, E>>.Default.Equals(left, right);
    }

    public static bool operator !=(AMyLocalization<S, L, E>? left, AMyLocalization<S, L, E>? right) {
        return !(left == right);
    }
}
