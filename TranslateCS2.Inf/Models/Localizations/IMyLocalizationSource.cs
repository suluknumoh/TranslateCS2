using System.Collections.Generic;

namespace TranslateCS2.Inf.Models.Localizations;
public interface IMyLocalizationSource<L, E> where L : ICollection<KeyValuePair<string, E>> {
    L? Localizations { get; }
    IDictionary<string, int> Indices { get; }
}