using System.IO;
using System.Text;

using Newtonsoft.Json;

namespace TranslateCS2.Mod.Helpers;
internal static class JsonHelper {
    public static void Write(object obj, string path) {
        string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
        File.WriteAllText(path, json, Encoding.UTF8);
    }
}
