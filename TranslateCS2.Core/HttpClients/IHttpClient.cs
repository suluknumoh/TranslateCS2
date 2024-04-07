using System;
using System.Diagnostics.CodeAnalysis;
using System.IO;
using System.Net.Http;
using System.Runtime.Versioning;
using System.Threading;
using System.Threading.Tasks;

namespace TranslateCS2.Core.HttpClients;
/// <summary>
///     wrapper for <see cref="HttpClient"/>
///     <br/>
///     <seealso href="https://learn.microsoft.com/de-de/dotnet/fundamentals/networking/http/httpclient-guidelines#recommended-use"/>
/// </summary>
public interface IHttpClient {
    void CancelPendingRequests();
    Task<HttpResponseMessage> DeleteAsync(Uri? requestUri);
    Task<HttpResponseMessage> DeleteAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken);
    Task<HttpResponseMessage> DeleteAsync([StringSyntax("Uri")] string? requestUri);
    Task<HttpResponseMessage> DeleteAsync(Uri? requestUri, CancellationToken cancellationToken);
    Task<HttpResponseMessage> GetAsync([StringSyntax("Uri")] string? requestUri, HttpCompletionOption completionOption);
    Task<HttpResponseMessage> GetAsync([StringSyntax("Uri")] string? requestUri);
    Task<HttpResponseMessage> GetAsync(Uri? requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken);
    Task<HttpResponseMessage> GetAsync(Uri? requestUri, HttpCompletionOption completionOption);
    Task<HttpResponseMessage> GetAsync(Uri? requestUri);
    Task<HttpResponseMessage> GetAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken);
    Task<HttpResponseMessage> GetAsync([StringSyntax("Uri")] string? requestUri, HttpCompletionOption completionOption, CancellationToken cancellationToken);
    Task<HttpResponseMessage> GetAsync(Uri? requestUri, CancellationToken cancellationToken);
    Task<byte[]> GetByteArrayAsync([StringSyntax("Uri")] string? requestUri);
    Task<byte[]> GetByteArrayAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken);
    Task<byte[]> GetByteArrayAsync(Uri? requestUri);
    Task<byte[]> GetByteArrayAsync(Uri? requestUri, CancellationToken cancellationToken);
    Task<Stream> GetStreamAsync([StringSyntax("Uri")] string? requestUri);
    Task<Stream> GetStreamAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken);
    Task<Stream> GetStreamAsync(Uri? requestUri);
    Task<Stream> GetStreamAsync(Uri? requestUri, CancellationToken cancellationToken);
    Task<string> GetStringAsync(Uri? requestUri);
    Task<string> GetStringAsync([StringSyntax("Uri")] string? requestUri);
    Task<string> GetStringAsync([StringSyntax("Uri")] string? requestUri, CancellationToken cancellationToken);
    Task<string> GetStringAsync(Uri? requestUri, CancellationToken cancellationToken);
    Task<HttpResponseMessage> PatchAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content, CancellationToken cancellationToken);
    Task<HttpResponseMessage> PatchAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content);
    Task<HttpResponseMessage> PatchAsync(Uri? requestUri, HttpContent? content);
    Task<HttpResponseMessage> PatchAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken);
    Task<HttpResponseMessage> PostAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content);
    Task<HttpResponseMessage> PostAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content, CancellationToken cancellationToken);
    Task<HttpResponseMessage> PostAsync(Uri? requestUri, HttpContent? content);
    Task<HttpResponseMessage> PostAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken);
    Task<HttpResponseMessage> PutAsync(Uri? requestUri, HttpContent? content);
    Task<HttpResponseMessage> PutAsync(Uri? requestUri, HttpContent? content, CancellationToken cancellationToken);
    Task<HttpResponseMessage> PutAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content);
    Task<HttpResponseMessage> PutAsync([StringSyntax("Uri")] string? requestUri, HttpContent? content, CancellationToken cancellationToken);
    [UnsupportedOSPlatform("browser")]
    HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken);
    [UnsupportedOSPlatform("browser")]
    HttpResponseMessage Send(HttpRequestMessage request, CancellationToken cancellationToken);
    [UnsupportedOSPlatform("browser")]
    HttpResponseMessage Send(HttpRequestMessage request, HttpCompletionOption completionOption);
    [UnsupportedOSPlatform("browser")]
    HttpResponseMessage Send(HttpRequestMessage request);
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken);
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request);
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption);
    Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, HttpCompletionOption completionOption, CancellationToken cancellationToken);
}