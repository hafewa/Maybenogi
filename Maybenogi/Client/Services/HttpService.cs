using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Maybenogi.Shared.Utils;

namespace Maybenogi.Client.Services
{
    public interface IHttpService
    {
        //Task<T> GetFromJsonAsync<T>(string uri);
        //Task<T> PostAsJsonAsync<T>(string uri, T content);

    }

    public class HttpService : IHttpService
    {
        //private HttpClient _httpClient;

        //public HttpService(HttpClient httpClient)
        //{
        //    this._httpClient = httpClient;
        //}

        //public async Task<T> GetFromJsonAsync<T>(string uri)
        //{
        //    return await this._httpClient.GetFromJsonAsync<T>(uri);
        //}

        //public async Task<T> PostAsJsonAsync<T>(string uri, T content)
        //{
        //    var request = new HttpRequestMessage(HttpMethod.Post, uri);
        //    request.Content = new StringContent(JsonSerializer.Serialize(content), Encoding.UTF8, "application/json");

        //    HttpResponseMessage response = await _httpClient.SendAsync(request);

        //    var options = new JsonSerializerOptions();
        //    options.PropertyNameCaseInsensitive = true;
        //    //options.Converters.Add(new StringConverter());

        //    return await response.Content.ReadFromJsonAsync<T>(options);
        //}
    }
}
