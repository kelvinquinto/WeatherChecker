using System.Threading.Tasks;

namespace WeatherChecker.Inteface
{
    public interface IWeatherBl
    {
        Task GetWeather(int intZipCode);
    }
}