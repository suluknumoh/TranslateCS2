using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace TranslateCS2.Core.HttpClients;
internal class AppHttpClient : IHttpClient {
    private HttpClient HttpClient { get; }
    public AppHttpClient(HttpClient httpClient) {
        this.HttpClient = httpClient;
    }
    public void CancelPendingRequests() {
        this.HttpClient.CancelPendingRequests();
    }
    public Task<HttpResponseMessage> DeleteAsync(Uri? requestUri) {
        return this.HttpClient.DeleteAsync(requestUri);
    }
    public Task<HttpResponseMessage> DeleteAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.DeleteAsync(requestUri, cancellationToken);
    }
    public Task<HttpResponseMessage> DeleteAsync([StringSyntax("Uri")] string? requestUri) {
        return this.HttpClient.DeleteAsync(requestUri);
    }
    public Task<HttpResponseMessage> DeleteAsync(Uri? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.DeleteAsync(requestUri, cancellationToken);
    }
    public Task<HttpResponseMessage> GetAsync([StringSyntax("Uri")] string? requestUri, HttpCompletionOption completionOption) {
        return this.HttpClient.GetAsync(requestUri, completionOption);
    }
    public Task<HttpResponseMessage> GetAsync([StringSyntax("Uri")] string? requestUri) {
        return this.HttpClient.GetAsync(requestUri);
    }
    public Task<HttpResponseMessage> GetAsync(Uri? requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken) {
        return this.HttpClient.GetAsync(requestUri, completionOption, cancellationToken);
    }
    public Task<HttpResponseMessage> GetAsync(Uri? requestUri, HttpCompletionOption completionOption) {
        return this.HttpClient.GetAsync(requestUri, completionOption);
    }
    public Task<HttpResponseMessage> GetAsync(Uri? requestUri) {
        return this.HttpClient.GetAsync(requestUri);
    }
    public Task<HttpResponseMessage> GetAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.GetAsync(requestUri, cancellationToken);
    }
    public Task<HttpResponseMessage> GetAsync([StringSyntax("Uri")] string? requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken) {
        return this.HttpClient.GetAsync(requestUri, completionOption, cancellationToken);
    }
    public Task<HttpResponseMessage> GetAsync(Uri? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.GetAsync(requestUri, cancellationToken);
    }
    public Task<byte[]> GetByteArrayAsync([StringSyntax("Uri")] string? requestUri) {
        return this.HttpClient.GetByteArrayAsync(requestUri);
    }
    public Task<byte[]> GetByteArrayAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.GetByteArrayAsync(requestUri, cancellationToken);
    }
    public Task<byte[]> GetByteArrayAsync(Uri? requestUri) {
        return this.HttpClient.GetByteArrayAsync(requestUri);
    }
    public Task<byte[]> GetByteArrayAsync(Uri? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.GetByteArrayAsync(requestUri, cancellationToken);
    }
    public Task<Stream> GetStreamAsync([StringSyntax("Uri")] string? requestUri) {
        return this.HttpClient.GetStreamAsync(requestUri);
    }
    public Task<Stream> GetStreamAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.GetStreamAsync(requestUri, cancellationToken);
    }
    public Task<Stream> GetStreamAsync(Uri? requestUri) {
        return this.HttpClient.GetStreamAsync(requestUri);
    }
    public Task<Stream> GetStreamAsync(Uri? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.GetStreamAsync(requestUri, cancellationToken);
    }
    public Task<string> GetStringAsync(Uri? requestUri) {
        return this.HttpClient.GetStringAsync(requestUri);
    }
    public Task<string> GetStringAsync([StringSyntax("Uri")] string? requestUri) {
        return this.HttpClient.GetStringAsync(requestUri);
    }
    public Task<string> GetStringAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.GetStringAsync(requestUri, cancellationToken);
    }
    public Task<string> GetStringAsync(Uri? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.GetStringAsync(requestUri, cancellationToken);
    }
    public Task<HttpResponseMessage> PatchAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content, CancellationToken cancellationToken) {
        return this.HttpClient.PatchAsync(requestUri, content, cancellationToken);
    }
    public Task<HttpResponseMessage> PatchAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content) {
        return this.HttpClient.PatchAsync(requestUri, content);
    }
    public Task<HttpResponseMessage> PatchAsync(Uri? requestUri, HttpContent? content) {
        return this.HttpClient.PatchAsync(requestUri, content);
    }
    public Task<HttpResponseMessage> PatchAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken) {
        return this.HttpClient.PatchAsync(requestUri, content, cancellationToken);
    }
    public Task<HttpResponseMessage> PostAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content) {
        return this.HttpClient.PostAsync(requestUri, content);
    }
    public Task<HttpResponseMessage> PostAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content, CancellationToken cancellationToken) {
        return this.HttpClient.PostAsync(requestUri, content, cancellationToken);
    }
    public Task<HttpResponseMessage> PostAsync(Uri? requestUri, HttpContent? content) {
        return this.HttpClient.PostAsync(requestUri, content);
    }
    public Task<HttpResponseMessage> PostAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken) {
        return this.HttpClient.PostAsync(requestUri, content, cancellationToken);
    }
    public Task<HttpResponseMessage> PutAsync(Uri? requestUri, HttpContent? content) {
        return this.HttpClient.PutAsync(requestUri, content);
    }
    public Task<HttpResponseMessage> PutAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken) {
        return this.HttpClient.PutAsync(requestUri, content, cancellationToken);
    }
    public Task<HttpResponseMessage> PutAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content) {
        return this.HttpClient.PutAsync(requestUri, content);
    }
    public Task<HttpResponseMessage> PutAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content, CancellationToken cancellationToken) {
        return this.HttpClient.PutAsync(requestUri, content, cancellationToken);
    }
    [UnsupportedOSPlatform("browser")]
    public HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken) {
        return this.HttpClient.Send(request, completionOption, cancellationToken);
    }
    [UnsupportedOSPlatform("browser")]
    public HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken) {
        return this.HttpClient.Send(request, cancellationToken);
    }
    [UnsupportedOSPlatform("browser")]
    public HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption) {
        return this.HttpClient.Send(request, completionOption);
    }
    [UnsupportedOSPlatform("browser")]
    public HttpResponseMessage Send(HttpRequestMessage request) {
        return this.HttpClient.Send(request);
    }
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        return this.HttpClient.SendAsync(request, cancellationToken);
    }
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) {
        return this.HttpClient.SendAsync(request);
    }
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption) {
        return this.HttpClient.SendAsync(request, completionOption);
    }
    public Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken) {
        return this.HttpClient.SendAsync(request, completionOption, cancellationToken);
    }
}