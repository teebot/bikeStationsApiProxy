using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Runtime.Serialization;

namespace BikeProxy.Api.Models
{
    public class Station {

        public string id { get; set; }

        public string stationName { get; set; }

        public string stAddress1 { get; set; }

        public decimal latitude { get; set; }

        public decimal longitude { get; set; }

        public int availableBikes { get; set; }

        public int availableDocks { get; set; }
    }
        

    public class ProxyStation
    {
        public string number { get; set; }

        public string name { get; set; }

        public string address { get; set; }

        public int available_bike_stands { get; set; }

        public int available_bikes { get; set; }

        public ProxyPosition position { get; set; }

        public Station ConvertToModel()
        {
            return new Station() { 
                id = number,
                stationName = FilterName(name),
                stAddress1 = address,
                latitude = position.lat,
                longitude = position.lng,
                availableBikes = available_bikes,
                availableDocks = available_bike_stands
            };
        }

        private string FilterName(string name)
        {
            if (name.Contains(" - "))
            {
                var parts = name.Split(new[] {" - " }, StringSplitOptions.None);
                return string.Concat(parts.Skip(1));
            }
            return name;
        }
    }

    public class ProxyPosition
    {
        public decimal lat { get; set; }

        public decimal lng { get; set; }
    }
}