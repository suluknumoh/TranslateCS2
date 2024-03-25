﻿using System;
using System.Net;
using System.Net.Http;
using System.Reflection;

using TranslateCS2.Configurations;

namespace TranslateCS2.Services;
internal class LatestVersionCheckService {
    private readonly HttpClient _httpClient;
    public Version Latest { get; private set; }
    public Version Current { get; private set; }
    public LatestVersionCheckService(HttpClient httpClient) {
        this._httpClient = httpClient;
        this.Current = new Version("0.0.0.0");
        this.Latest = this.Current;
    }
    public bool IsNewVersionAvailable() {
        Assembly assembly = Assembly.GetExecutingAssembly();
        this.Current = assembly.GetName().Version ?? new Version("0.0.0.0");
        this.Latest = this.Current;
        try {
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, AppConfigurationManager.CheckLatestURL);
            request.Headers.Add(HttpRequestHeader.Accept.ToString(), "text/plain");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Headers.Add("Sec-GPC", "1");
            HttpResponseMessage response = this._httpClient.Send(request);
            response.EnsureSuccessStatusCode();
            HttpContent content = response.Content;
            string contentString = content.ReadAsStringAsync().GetAwaiter().GetResult();
            string versionString = contentString.Trim().Replace("AssemblyVersion", String.Empty).Replace("<", String.Empty).Replace(">", String.Empty).Replace("/", String.Empty);
            this.Latest = new Version(versionString);
            return this.Latest > this.Current;
        } catch {
            // nüx
            return false;
        }
    }
}
