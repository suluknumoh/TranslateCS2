using TranslateCS2.Inf.Attributes;

namespace TranslateCS2.Inf;
[MyExcludeFromCoverage]
public static class LoggingConstants {
    public static string FailedTo => "failed to:";
    public static string StrangerThings => $"{FailedTo} load the entire mod:";
    public static string StrangerThingsDispose => $"{FailedTo} dispose:";
    public static string NoCultureInfo => "no culture info:";
    public static string CorruptIndexedKeyValue => "the following Key-Value-Pairs are not in the correct order and dismissed:";

}
