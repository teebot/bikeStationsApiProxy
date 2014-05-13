using BikeProxy.Api.Models;
using Newtonsoft.Json;
using NLog;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;
using System.Web;

namespace BikeProxy.Api.Callers
{
    /// <summary>
    /// A cached api client for JCDecaux
    /// </summary>
    public class JCDecauxClient
    {
        private readonly int _apiCacheExpirationInMinutes;
        private readonly string _baseApiUrl;
        private readonly string _apiKey;
        private readonly string _contractName;
        private static Logger logger = LogManager.GetCurrentClassLogger();
        
        private static JCDecauxClient _instance;
        public static JCDecauxClient Instance
        {
            get
            {
                if (_instance == null) 
                    _instance = new JCDecauxClient();

                return _instance;
            }
        }


        private DateTime _lastCall;

        private IEnumerable<ProxyStation> _stations;

        private JCDecauxClient()
        {
            _baseApiUrl = ConfigurationManager.AppSettings["JCDecauxRootApiUrl"];
            _apiKey = ConfigurationManager.AppSettings["JCDecauxApiKey"];
            _contractName = ConfigurationManager.AppSettings["JCDecauxContractName"];
            _apiCacheExpirationInMinutes = int.Parse(ConfigurationManager.AppSettings["ApiCacheExpirationInMinutes"]);
        }

        public IEnumerable<ProxyStation> Stations
        {
            get {
                if (_stations == null || _lastCall == null || _lastCall < DateTime.Now.AddMinutes(-_apiCacheExpirationInMinutes))
                {
                    UpdateStationsFromApi();
                }

                return _stations;
            }
        }

        private void UpdateStationsFromApi()
        {
            var requestUri = string.Format("stations");
            _stations = GetCall<IEnumerable<ProxyStation>>(requestUri);
        }

        private T GetCall<T>(string method)
        {
            try
            {
                var requestUri = string.Format("/vls/v1/{0}?contract={1}&apiKey={2}", method, _contractName, _apiKey);
                using (var client = new HttpClient())
                {
                    client.BaseAddress = new Uri(_baseApiUrl);
                    client.DefaultRequestHeaders.Accept.Clear();
                    client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

                    HttpResponseMessage response = client.GetAsync(requestUri).Result;

                    // will throw an exception if http status code is an error
                    response.EnsureSuccessStatusCode(); 

                    _lastCall = DateTime.Now;
                    //return response.Content.ReadAsAsync<T>().Result;

                    return JsonConvert.DeserializeObject<T>(response.Content.ReadAsStringAsync().Result);
                }
            }
            catch (Exception e)
            {
                logger.LogException(LogLevel.Error, "api get call failed", e);
            }

            return default(T);
        }
    }
}