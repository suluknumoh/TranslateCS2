using System.IO;
using System.Reflection;
using System.Text;

using Newtonsoft.Json;

namespace TranslateCS2.Mod.Helpers;
internal static class JsonHelper {
    public static void Write(object obj, string path) {
        string json = JsonConvert.SerializeObject(obj, Formatting.Indented);
        File.WriteAllText(path, json, Encoding.UTF8);
    }

    public static T? DeSerializeFromAssembly<T>(string path) {
        Assembly assembly = typeof(JsonHelper).Assembly;
        using Stream stream = assembly.GetManifestResourceStream(path);
        return DeSerializeFromStream<T>(stream);
    }

    public static T? DeSerializeFromStream<T>(Stream stream) {
        JsonSerializer serializer = JsonSerializer.CreateDefault();
        using TextReader textReader = new StreamReader(stream, Encoding.UTF8);
        using JsonReader jsonReader = new JsonTextReader(textReader);
        return serializer.Deserialize<T>(jsonReader);
    }
}
