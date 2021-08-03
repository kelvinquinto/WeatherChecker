using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using WeatherChecker.Inteface;
using WeatherChecker.Model;

namespace WeatherChecker.Implementation
{
    public class WeatherBl : IWeatherBl
    {
        private readonly ILogger<WeatherBl> _log;
        private readonly IConfiguration _config;

        public WeatherBl(ILogger<WeatherBl> log, IConfiguration config)
        {
            _log = log;
            _config = config;
        }

        public async Task GetWeather(int intZipCode)
        {
            try
            {
                string url = string.Format(_config.GetValue<string>("URL"), intZipCode);
                var httpClient = HttpClientFactory.Create();

                HttpResponseMessage response = await httpClient.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.OK)
                {
                    var content = response.Content;
                    var data = await content.ReadAsStringAsync();

                    WeatherModel model = JsonConvert.DeserializeObject<WeatherModel>(data);

                    if (model != null && model.current != null)
                    {
                        bool ysnRaining = false;
                        ysnRaining = model.current.weather_descriptions.Contains("rain");

                        _log.LogInformation("Should I go outside? {result}", !ysnRaining ? "YES" : "NO");
                        _log.LogInformation("Should I wear sunscreen? {result}", model.current.uv_index > 3 ? "YES" : "NO");
                        _log.LogInformation("Can I fly my kite? {result}", model.current.wind_speed > 15 && !ysnRaining ? "YES" : "NO");
                    }
                    else
                        _log.LogInformation("Unable to find Weather Status for Zip Code {result}", intZipCode);
                }
            }
            catch (Exception ex)
            {
                _log.LogInformation(ex.Message);
            }
        }
    }
}
