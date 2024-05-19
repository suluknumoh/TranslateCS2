using System;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;

using TranslateCS2.Core.Configurations;

namespace TranslateCS2.Core.Services.LatestVersions;
internal class LatestVersionCheckService : ILatestVersionCheckService {
    private readonly HttpClient httpClient;
    public Version Latest { get; private set; }
    public Version Current { get; private set; }
    public LatestVersionCheckService(HttpClient httpClient) {
        this.httpClient = httpClient;
        this.Current = new Version("0.0.0.0");
        this.Latest = this.Current;
    }
    public async Task<bool> IsNewVersionAvailable() {
        Assembly? assembly = Assembly.GetEntryAssembly();
        this.Current = assembly?.GetName().Version ?? new Version("0.0.0.0");
        this.Latest = this.Current;
        try {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, AppConfigurationManager.CheckLatestURL);
            request.Headers.Add(HttpRequestHeader.Accept.ToString(), "text/plain");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Headers.Add("Sec-GPC", "1");
            HttpResponseMessage response = await this.httpClient.SendAsync(request);
            response.EnsureSuccessStatusCode();
            HttpContent content = response.Content;
            string contentString = await content.ReadAsStringAsync();
            string versionString = contentString.Trim();
            this.Latest = new Version(versionString);
            return this.Latest > this.Current;
        } catch {
            // nüx
            return false;
        }
    }
}
