using System.Diagnostics;

namespace TranslateCS2.Core.Helpers;
public static class URLHelper {
    public static void Open(string url) {
        Process.Start(new ProcessStartInfo(url) {
            UseShellExecute = true
        });
    }
}
