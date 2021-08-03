using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WeatherChecker.Model
{
    public class Location
    {
        public string name { get; set; }
        public string country { get; set; }
        public string region { get; set; }
        public decimal lat { get; set; }
        public decimal lon { get; set; }
        public string timezone_id { get; set; }
        public DateTime localtime { get; set; }
        public int localtime_epoch { get; set; }
        public decimal utc_offset { get; set; }
    }
}
