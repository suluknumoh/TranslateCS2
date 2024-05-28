using System.Collections.Generic;
using System.IO;
using System.Text;

using TranslateCS2.Inf.Attributes;

namespace TranslateCS2.Inf.Models.Localizations;
public class MyLocalizationSource<E> {
    public FileInfo File { get; }
    public IDictionary<string, E> Localizations { get; } = new Dictionary<string, E>();
    public IDictionary<string, int> IndexCounts { get; } = new Dictionary<string, int>();
    public MyLocalizationSource(FileInfo file) {
        this.File = file;
    }
    [MyExcludeFromCoverage]
    public override string ToString() {
        StringBuilder builder = new StringBuilder();
        builder.AppendLine(nameof(MyLocalization<E>));
        builder.Append(this.File.ToString());
        return builder.ToString();
    }
}
