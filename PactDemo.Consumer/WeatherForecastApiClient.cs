using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace PactDemo.Consumer
{
    public class WeatherForecastApiClient
    {
        private readonly HttpClient _client;

        public WeatherForecastApiClient(string baseUri = null)
            : this(new HttpClient {BaseAddress = new Uri(baseUri ?? "http://my-api")})
        {
        }

        public WeatherForecastApiClient(HttpClient client)
        {
            _client = client;
        }

        public async Task<WeatherForecastResponse> GetForecastTomorrow()
        {
            string reasonPhrase;

            var request = new HttpRequestMessage(HttpMethod.Get, "/WeatherForecast/tomorrow");
            request.Headers.Add("Accept", "application/json");

            var response = await _client.SendAsync(request);

            var content = await response.Content.ReadAsStringAsync();
            var status = response.StatusCode;

            reasonPhrase =
                response
                    .ReasonPhrase; //NOTE: any Pact mock provider errors will be returned here and in the response body

            request.Dispose();
            response.Dispose();

            if (status == HttpStatusCode.OK)
            {
                return !string.IsNullOrEmpty(content)
                    ? JsonConvert.DeserializeObject<WeatherForecastResponse>(content)
                    : null;
            }

            throw new Exception(reasonPhrase);
        }
    }
}