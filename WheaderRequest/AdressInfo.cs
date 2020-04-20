using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
namespace WheaderRequest
{
    public class AdressInfo
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        [JsonIgnore]
        public int Id { get; set; }
 
        [JsonProperty("place_id")]
        public string PlaceId { get; set; }
        [JsonProperty("licence")]
        public string Licence { get; set; }
        [JsonProperty("osm_type")]
        public string OsmType { get; set; }
        [JsonProperty("osm_id")]
        public string OsmId { get; set; }
        [NotMapped]
        [JsonProperty("boundingbox")]
        public List<string> BoundingBox { get; set; }
        [JsonProperty("lat")]
        public string Lat { get; set; }
        [JsonProperty("lon")]
        public string Lon { get; set; }
        [JsonProperty("display_name")]
        public string DisplayName { get; set; }
        [JsonProperty("class")]
        public string Class { get; set; }
        [JsonProperty("type")]
        public string Type { get; set; }
        [JsonProperty("importance")]
        public double Importance { get; set; }
        [JsonIgnore]
        public int RequestInfoId { get; set; }
    }

    [JsonObject(MemberSerialization.OptOut)]
    public class AdressInfoWsResponce
    {
        [JsonProperty("processinfo")]
        public BaseResponse BaseResponse;
        [JsonProperty("summaryData")]
        public SummaryData SummaryData{ get; set; }
    }
}
