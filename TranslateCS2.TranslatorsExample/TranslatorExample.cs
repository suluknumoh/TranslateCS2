using System;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

using TranslateCS2.Core.Configurations;
using TranslateCS2.Core.Translators;

namespace TranslateCS2.TranslatorsExample;
internal class TranslatorExample : ATranslator {
    public TranslatorExample(string name, string description) : base(name, description) {
        // no need to call Init()
        // no need to provide an HttpClient
        // Translators receive it from the app itself!!!
    }
    public override async Task InitAsync(HttpClient httpClient) {
        // this method is called by the app itself!!!
        //
        // this is just an example-list
        // this list should contain language-codes or some other text related to the used translator-api
        // the selected ones gets reflected into
        /// <see cref="SelectedTargetLanguageCode"/>
        if (false) {
            // example-usage!
            // you should retrieve the available language codes from the api you'd like to use
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, AppConfigurationManager.CheckLatestURL);
            request.Headers.Add(HttpRequestHeader.Accept.ToString(), "text/plain");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Headers.Add("Sec-GPC", "1");
            HttpResponseMessage response = httpClient.Send(request);
            response.EnsureSuccessStatusCode();
            HttpContent content = response.Content;
            string contentString = await content.ReadAsStringAsync();
        }

        this.TargetLanguageCodes.Add("AR - Arabic");
        this.TargetLanguageCodes.Add("BG - Bulgarian");
        this.TargetLanguageCodes.Add("CS - Czech");
        this.TargetLanguageCodes.Add("DA - Danish");
        this.TargetLanguageCodes.Add("DE - German");
        this.TargetLanguageCodes.Add("EL - Greek");
        this.TargetLanguageCodes.Add("EN - English");
        this.TargetLanguageCodes.Add("ES - Spanish");
        this.TargetLanguageCodes.Add("ET - Estonian");
        this.TargetLanguageCodes.Add("FI - Finnish");
        this.TargetLanguageCodes.Add("FR - French");
        this.TargetLanguageCodes.Add("HU - Hungarian");
        this.TargetLanguageCodes.Add("ID - Indonesian");
        this.TargetLanguageCodes.Add("IT - Italian");
        this.TargetLanguageCodes.Add("JA - Japanese");
        this.TargetLanguageCodes.Add("KO - Korean");
        this.TargetLanguageCodes.Add("LT - Lithuanian");
        this.TargetLanguageCodes.Add("LV - Latvian");
        this.TargetLanguageCodes.Add("NB - Norwegian");
        this.TargetLanguageCodes.Add("NL - Dutch");
        this.TargetLanguageCodes.Add("PL - Polish");
        this.TargetLanguageCodes.Add("PT - Portuguese");
        this.TargetLanguageCodes.Add("RO - Romanian");
        this.TargetLanguageCodes.Add("RU - Russian");
        this.TargetLanguageCodes.Add("SK - Slovak");
        this.TargetLanguageCodes.Add("SL - Slovenian");
        this.TargetLanguageCodes.Add("SV - Swedish");
        this.TargetLanguageCodes.Add("TR - Turkish");
        this.TargetLanguageCodes.Add("UK - Ukrainian");
        this.TargetLanguageCodes.Add("ZH - Chinese");
    }

    public override async Task<TranslatorResult> TranslateAsync(HttpClient httpClient, string sourceLanguageCode, string? s) {
        if (String.IsNullOrEmpty(s) || String.IsNullOrWhiteSpace(s)) {
            return new TranslatorResult() { Error = "nothing to translate; text to translate is empty" };
        }

        if (false) {
            // example-usage!
            // you should call the respective translate-api-endpoint
            HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, AppConfigurationManager.CheckLatestURL);
            request.Headers.Add(HttpRequestHeader.Accept.ToString(), "text/plain");
            request.Headers.Add("Upgrade-Insecure-Requests", "1");
            request.Headers.Add("Sec-GPC", "1");
            HttpResponseMessage response = httpClient.Send(request);
            response.EnsureSuccessStatusCode();
            HttpContent content = response.Content;
            string contentString = await content.ReadAsStringAsync();
        }


        /// <see cref="SelectedTargetLanguageCode"/>
        /// is the selected language code from
        /// <see cref="TargetLanguageCodes"/>
        StringBuilder builder = new StringBuilder();
        builder.AppendLine("this is just an example");
        builder.AppendLine("nothing got translated!!!");
        builder.AppendLine($"'{this.Name}' translated '{s}' from '{sourceLanguageCode}' into '{this.SelectedTargetLanguageCode}'");
        return new TranslatorResult() { Translation = builder.ToString() };
    }
}
