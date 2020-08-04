using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace PactDemo.Consumer
{
    class Program
    {
        static void Main(string[] args)
        {
            var forecastInt = new ForecastIntegration();
            var foreCastResult = forecastInt.GetForecast().Result;
            Console.WriteLine("The forecast is {0}", foreCastResult);
        }
    }

    public class ForecastIntegration
    {
        public async Task<string> GetForecast()
        {
            // var httpClientHandler = new HttpClientHandler();
            // httpClientHandler.ServerCertificateCustomValidationCallback =
            //     (message, cert, chain, errors) => true; // DEBUGGING ONLY
            //
            // using var ht = new HttpClient(httpClientHandler);
            // ht.BaseAddress = new Uri("http://localhost:5000");
            
            var apiClient = new WeatherForecastApiClient("http://localhost:5000");

            var forecast = await apiClient.GetForecastTomorrow();

            return forecast.Summary;
        }
    }
}