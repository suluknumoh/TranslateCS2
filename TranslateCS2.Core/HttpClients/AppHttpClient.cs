using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace TranslateCS2.Core.HttpClients;
public sealed class AppHttpClient : HttpClient, IHttpClient {
    private HttpClient HttpClient { get; }
    /// <inheritdoc/>
    public HttpClient Underlying => this;
    internal AppHttpClient(HttpClient httpClient) {
        this.HttpClient = httpClient;
    }
    public new void CancelPendingRequests() {
        this.HttpClient.CancelPendingRequests();
    }
    public new Task<HttpResponseMessage> DeleteAsync(Uri? requestUri) {
        return this.HttpClient.DeleteAsync(requestUri);
    }
    public new Task<HttpResponseMessage> DeleteAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.DeleteAsync(requestUri, cancellationToken);
    }
    public new Task<HttpResponseMessage> DeleteAsync([StringSyntax("Uri")] string? requestUri) {
        return this.HttpClient.DeleteAsync(requestUri);
    }
    public new Task<HttpResponseMessage> DeleteAsync(Uri? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.DeleteAsync(requestUri, cancellationToken);
    }
    public new Task<HttpResponseMessage> GetAsync([StringSyntax("Uri")] string? requestUri, HttpCompletionOption completionOption) {
        return this.HttpClient.GetAsync(requestUri, completionOption);
    }
    public new Task<HttpResponseMessage> GetAsync([StringSyntax("Uri")] string? requestUri) {
        return this.HttpClient.GetAsync(requestUri);
    }
    public new Task<HttpResponseMessage> GetAsync(Uri? requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken) {
        return this.HttpClient.GetAsync(requestUri, completionOption, cancellationToken);
    }
    public new Task<HttpResponseMessage> GetAsync(Uri? requestUri, HttpCompletionOption completionOption) {
        return this.HttpClient.GetAsync(requestUri, completionOption);
    }
    public new Task<HttpResponseMessage> GetAsync(Uri? requestUri) {
        return this.HttpClient.GetAsync(requestUri);
    }
    public new Task<HttpResponseMessage> GetAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.GetAsync(requestUri, cancellationToken);
    }
    public new Task<HttpResponseMessage> GetAsync([StringSyntax("Uri")] string? requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken) {
        return this.HttpClient.GetAsync(requestUri, completionOption, cancellationToken);
    }
    public new Task<HttpResponseMessage> GetAsync(Uri? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.GetAsync(requestUri, cancellationToken);
    }
    public new Task<byte[]> GetByteArrayAsync([StringSyntax("Uri")] string? requestUri) {
        return this.HttpClient.GetByteArrayAsync(requestUri);
    }
    public new Task<byte[]> GetByteArrayAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.GetByteArrayAsync(requestUri, cancellationToken);
    }
    public new Task<byte[]> GetByteArrayAsync(Uri? requestUri) {
        return this.HttpClient.GetByteArrayAsync(requestUri);
    }
    public new Task<byte[]> GetByteArrayAsync(Uri? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.GetByteArrayAsync(requestUri, cancellationToken);
    }
    public new Task<Stream> GetStreamAsync([StringSyntax("Uri")] string? requestUri) {
        return this.HttpClient.GetStreamAsync(requestUri);
    }
    public new Task<Stream> GetStreamAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.GetStreamAsync(requestUri, cancellationToken);
    }
    public new Task<Stream> GetStreamAsync(Uri? requestUri) {
        return this.HttpClient.GetStreamAsync(requestUri);
    }
    public new Task<Stream> GetStreamAsync(Uri? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.GetStreamAsync(requestUri, cancellationToken);
    }
    public new Task<string> GetStringAsync(Uri? requestUri) {
        return this.HttpClient.GetStringAsync(requestUri);
    }
    public new Task<string> GetStringAsync([StringSyntax("Uri")] string? requestUri) {
        return this.HttpClient.GetStringAsync(requestUri);
    }
    public new Task<string> GetStringAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.GetStringAsync(requestUri, cancellationToken);
    }
    public new Task<string> GetStringAsync(Uri? requestUri, CancellationToken cancellationToken) {
        return this.HttpClient.GetStringAsync(requestUri, cancellationToken);
    }
    public new Task<HttpResponseMessage> PatchAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content, CancellationToken cancellationToken) {
        return this.HttpClient.PatchAsync(requestUri, content, cancellationToken);
    }
    public new Task<HttpResponseMessage> PatchAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content) {
        return this.HttpClient.PatchAsync(requestUri, content);
    }
    public new Task<HttpResponseMessage> PatchAsync(Uri? requestUri, HttpContent? content) {
        return this.HttpClient.PatchAsync(requestUri, content);
    }
    public new Task<HttpResponseMessage> PatchAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken) {
        return this.HttpClient.PatchAsync(requestUri, content, cancellationToken);
    }
    public new Task<HttpResponseMessage> PostAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content) {
        return this.HttpClient.PostAsync(requestUri, content);
    }
    public new Task<HttpResponseMessage> PostAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content, CancellationToken cancellationToken) {
        return this.HttpClient.PostAsync(requestUri, content, cancellationToken);
    }
    public new Task<HttpResponseMessage> PostAsync(Uri? requestUri, HttpContent? content) {
        return this.HttpClient.PostAsync(requestUri, content);
    }
    public new Task<HttpResponseMessage> PostAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken) {
        return this.HttpClient.PostAsync(requestUri, content, cancellationToken);
    }
    public new Task<HttpResponseMessage> PutAsync(Uri? requestUri, HttpContent? content) {
        return this.HttpClient.PutAsync(requestUri, content);
    }
    public new Task<HttpResponseMessage> PutAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken) {
        return this.HttpClient.PutAsync(requestUri, content, cancellationToken);
    }
    public new Task<HttpResponseMessage> PutAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content) {
        return this.HttpClient.PutAsync(requestUri, content);
    }
    public new Task<HttpResponseMessage> PutAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content, CancellationToken cancellationToken) {
        return this.HttpClient.PutAsync(requestUri, content, cancellationToken);
    }
    [UnsupportedOSPlatform("browser")]
    public new HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken) {
        return this.HttpClient.Send(request, completionOption, cancellationToken);
    }
    [UnsupportedOSPlatform("browser")]
    public new HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken) {
        return this.HttpClient.Send(request, cancellationToken);
    }
    [UnsupportedOSPlatform("browser")]
    public new HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption) {
        return this.HttpClient.Send(request, completionOption);
    }
    [UnsupportedOSPlatform("browser")]
    public new HttpResponseMessage Send(HttpRequestMessage request) {
        return this.HttpClient.Send(request);
    }
    public new Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken) {
        return this.HttpClient.SendAsync(request, cancellationToken);
    }
    public new Task<HttpResponseMessage> SendAsync(HttpRequestMessage request) {
        return this.HttpClient.SendAsync(request);
    }
    public new Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption) {
        return this.HttpClient.SendAsync(request, completionOption);
    }
    public new Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken) {
        return this.HttpClient.SendAsync(request, completionOption, cancellationToken);
    }
    protected override void Dispose(bool disposing) {
        // dont dispose
    }
}