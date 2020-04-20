using Microsoft.AspNetCore.Mvc.Filters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using System.Threading.Tasks;

namespace WheaderRequest
{
    public static class Common
    {
        public static T DownloadSerializedJsonData<T>(string url) where T : new()
        {

            using (var w = new WebClient())
            {
                var json_data = string.Empty;
                json_data = w.DownloadString(url);

                return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<T>(json_data) : new T();
            }
        }

        public static double WeeklyMinTempruter(Datum[] data)
        {
            double minTept = data[0].TemperatureMin;
            foreach (Datum item in data)
            {
                if (item.TemperatureMin < minTept)
                {
                    minTept = item.TemperatureMin;
                }
            }
            return minTept;
        }

        public static double WeeklyMaxTempruter(Datum[] data)
        {
            double maxTept = data[0].TemperatureMax;
            foreach (Datum item in data)
            {
                if (item.TemperatureMin > maxTept)
                {
                    maxTept = item.TemperatureMin;
                }
            }
            return maxTept;
        }

    }

    [JsonObject(MemberSerialization.OptOut)]
    public class BaseResponse
    {

        [JsonProperty("MessageInfo")]
        public string MessageInfo { get; set; }
        [JsonProperty("ErorInfo")]
        public ErorCode ErorInfo { get; set; }
    }
    public enum ErorCode
    {
        Error = 1,
        Success = 2
    }


}
