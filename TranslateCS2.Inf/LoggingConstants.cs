using System;

namespace TranslateCS2.Inf;
public static class LoggingConstants {
    public static string FailedTo => "failed to:";
    public static string StrangerThings => $"{FailedTo} load the entire mod:";
    public static string StrangerThingsDispose => $"{FailedTo} dispose:";
    public static string NoCultureInfo => "no culture info:";
    public static string CorruptIndexedKeyValue => String.Join(Environment.NewLine, CorruptIndexedKeyValueLines);
    private static string[] CorruptIndexedKeyValueLines => [
        "the following Key-Value-Pair seems to be an indexed ones,",
        "if it is not or should not be one,",
        "dismiss this message;",
        "otherwise the Key-Value-Pair is corrupt,",
        "and the key has to follow the format \"Key:UnsignedIntegerAsZeroBasedIndex\";",
        "it is also possible, that the used index is larger (out of range) than the key-group it belongs to:"
    ];

}
