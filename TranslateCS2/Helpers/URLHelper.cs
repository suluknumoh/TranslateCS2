using System.Diagnostics;

namespace TranslateCS2.Helpers;
internal static class URLHelper {
    public static void Open(string url) {
        Process.Start(new ProcessStartInfo(url) {
            UseShellExecute = true
        });
    }
}
