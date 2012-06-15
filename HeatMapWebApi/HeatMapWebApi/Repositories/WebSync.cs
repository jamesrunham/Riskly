using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;

using System.Web;

namespace HeatMapWebApi.Repositories
{
    public class WebSync
    {
        private const string addressSuffix = "heatmap";
        public static HttpResponseMessage GetNodes()
        {
            var httpClient = new HttpClient();

            var url = string.Format("/?pathContains={0}", "Reed");
            url += string.Format("&iteration={0}", "Trunk");
            url += string.Format("&depth={0}", "100");

            return httpClient.GetAsync(addressSuffix + url).Result;
        }
    }
}