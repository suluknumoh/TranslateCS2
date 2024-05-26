using System.Collections.Generic;

namespace TranslateCS2.Inf.Models.Localizations;
public class MyLocalizationSource<E> {
    public IDictionary<string, E> Localizations { get; } = new Dictionary<string, E>();
    public IDictionary<string, int> IndexCounts { get; } = new Dictionary<string, int>();
}
