using System;
using System.Collections.Generic;
using System.Linq;

namespace TranslateCS2.Inf;
public static class IndexCountHelper {
    public delegate void OnCorruptIndexedKeyValue(Exception exception,
                                                  IGrouping<(string, int), KeyValuePair<string, string>>? group,
                                                  KeyValuePair<string, string>? groupItem);
    public static void FillIndexCountsFromLocalizationDictionary(IDictionary<string, string> localizationDictionary,
                                                                 IDictionary<string, int> indexCounts,
                                                                 OnCorruptIndexedKeyValue? onCorruptIndexedKeyValue) {
        // TODO: have to test it
        IEnumerable<KeyValuePair<string, string>> indexedValues =
            localizationDictionary.Where(item => item.Key.Contains(":"));

        if (!indexedValues.Any()) {
            return;
        }
        try {

            // need to parse it to be able to order it by idx ascending
            IEnumerable<IGrouping<(string key, int idx), KeyValuePair<string, string>>> grouped =
                indexedValues
                    .GroupBy(item =>
                        (item.Key.Split(':')[0], Int32.Parse(item.Key.Split(':')[1]))
                    );

            if (!grouped.Any()) {
                return;
            }
            IOrderedEnumerable<IGrouping<(string key, int idx), KeyValuePair<string, string>>> ordered =
                grouped.OrderBy(item => item.Key.idx);


            // TODO: due to a lack of time, i have to write it in german
            // die aktuelle logik geht davon aus, dass das localizationDictionary alle eintraege beinhaltet
            // aber das ist bloedsinn
            // es kann durchaus sein, dass jemand beispielsweise nur weitere staedtenamen hinzufuegt
            // dann umfasst countNew nur die neu hinzugefuegten eintraege
            // diese waeren aber gueltig, solange die indizes der reihe nach, nach dem entsprechend letzten index liegen
            // bsp.:
            // staedtenamen hat 10 indizes
            // dann ist der letzte index 9
            // fuegt jemand einen staedtenamen im localizationDictionary hinzu: staedtenamen:10
            // dann ist der eintrag gueltig, indexCount muss aber von 10 auf 11 erhoeht werden
            foreach (IGrouping<(string key, int idx), KeyValuePair<string, string>> group in ordered) {
                int countNew = group.Count();
                string key = group.Key.key;

                bool isExisting = indexCounts.TryGetValue(key, out int countExisting);

                foreach (KeyValuePair<string, string> groupItem in group) {
                    if (StringHelper.IsNullOrWhiteSpaceOrEmpty(groupItem.Value)) {
                        //
                        //
                        --countNew;
                        // TODO: Exception
                        onCorruptIndexedKeyValue?.Invoke(new Exception(), group, groupItem);
                        continue;
                        //
                        //
                    }
                    if (group.Key.idx < 0) {
                        //
                        //
                        --countNew;
                        // TODO: Exception
                        onCorruptIndexedKeyValue?.Invoke(new Exception(), group, groupItem);
                        continue;
                        //
                        //
                    }
                    if (isExisting) {
                        if (group.Key.idx >= countExisting
                            && group.Key.idx >= countNew) {
                            //
                            //
                            --countNew;
                            // TODO: Exception
                            onCorruptIndexedKeyValue?.Invoke(new Exception(), group, groupItem);
                            continue;
                            //
                            //
                        }
                    } else if (group.Key.idx >= countNew) {
                        //
                        //
                        --countNew;
                        // TODO: Exception
                        onCorruptIndexedKeyValue?.Invoke(new Exception(), group, groupItem);
                        continue;
                        //
                        //
                    }
                }

                if (countNew > 0) {
                    if (
                    //
                    // is 'new' indexed value
                    !isExisting
                    //
                    // localizationDictionary has more valid indexed values
                    || countExisting < countNew
                    //
                    ) {
                        // set new indexcount for key
                        indexCounts[key] = countNew;
                        //
                        // just to clarify:
                        // if localizationDictionary has less valid indexed values
                        // it does not matter,
                        // those that are present within this translation file are used
                        // others are taken from fallback/builtin
                    }
                }
            }
        } catch (Exception ex) {
            onCorruptIndexedKeyValue?.Invoke(ex, null, null);
        }
    }
}
