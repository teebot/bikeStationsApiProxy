using BikeProxy.Api.Callers;
using BikeProxy.Api.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;

namespace BikeProxy.Api.Controllers
{
    public class StationsController : ApiController
    {
        // GET api/stations
        public IEnumerable<Station> Get()
        {
            var stations = JCDecauxClient.Instance.Stations.Select(s => s.ConvertToModel()).ToList();
            return stations;
        }
    }
}
