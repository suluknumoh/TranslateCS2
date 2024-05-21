using System.Collections.Generic;

namespace TranslateCS2.Inf.Models.Localizations;
public interface IIndexCountsProvider {
    /// <summary>
    ///     adds the indexcounts that correspond to the given <paramref name="localeId"/> to the given <paramref name="indexCounts"/>
    /// </summary>
    /// <param name="indexCounts">
    ///     the <see cref="Dictionary{TKey, TValue}"/> to add the index counts to
    /// </param>
    /// <param name="localeId">
    ///     has to be a built in ones; one of the following
    ///     <br/>
    ///     <br/>
    ///     <list type="bullet">
    ///         <item>de-DE</item>
    ///         <item>en-US</item>
    ///         <item>es-ES</item>
    ///         <item>fr-FR</item>
    ///         <item>it-IT</item>
    ///         <item>ja-JP</item>
    ///         <item>ko-KR</item>
    ///         <item>pl-PL</item>
    ///         <item>pt-BR</item>
    ///         <item>ru-RU</item>
    ///         <item>zh-HANS</item>
    ///         <item>zh-HANT</item>
    ///     </list>
    /// </param>
    public void AddIndexCounts(Dictionary<string, int> indexCounts, string localeId);
}
