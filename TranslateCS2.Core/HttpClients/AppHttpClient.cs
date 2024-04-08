using System.Net.Http;

namespace TranslateCS2.Core.HttpClients;
public sealed class AppHttpClient : HttpClient {
    internal AppHttpClient() { }
    protected override void Dispose(bool disposing) {
        // dont dispose
    }
}