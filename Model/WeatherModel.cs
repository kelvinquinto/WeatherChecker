using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherChecker.Model
{
    public class WeatherModel
    {
        public Request request { get; set; }
        public Location location { get; set; }
        public Current current { get; set; }
    }
}
