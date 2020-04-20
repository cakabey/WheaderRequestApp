using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace WheaderRequestMonitoring
{
    public class  SummaryData
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public int ProcessId { get; set; }
        public DateTime DayDate { get; set; }

        public string PlaceId { get; set; }
        public string PlaceName { get; set; }
        public string Lat { get; set; }
        public string Lon { get; set; }
        public string DailyMinTemperature { get; set; }
        public string DailyMaxTemperature { get; set; }

        public string WeeklyMinTemperature { get; set; }
        public string WeeklyMaxTemperature { get; set; }

        public string Temperature { get; set; }

    }

    public class SummaryDataTable
    {
        public string PlaceName { get; set; }
        public string Temperature { get; set; }
        public string WeeklyMinTemperature { get; set; }
        public string WeeklyMaxTemperature { get; set; }
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class SummaryDataWsResponce
    {
        [JsonProperty("processinfo")]
        public BaseResponse BaseResponse;
        [JsonProperty("summaryData")]
        public SummaryData[] SummaryData { get; set; }
    }

    public class NewRequestData
    {
        public SummaryData SummaryData { get; set; }
        public double PastTime { get; set; }
        public string Input { get; set; }
    }

}
