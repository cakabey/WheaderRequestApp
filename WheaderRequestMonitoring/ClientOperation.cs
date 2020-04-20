using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Net;
using System.Text;

namespace WheaderRequestMonitoring
{

    public sealed class ClientOperation
    {
        private static ClientOperation instance = null;
        private static readonly object padlock = new object();

        ClientOperation()
        {
        }

        public static ClientOperation Instance
        {
            get
            {
                lock (padlock)
                {
                    if (instance == null)
                    {
                        instance = new ClientOperation();
                    }
                    return instance;
                }
            }
        }


        public SummaryDataWsResponce GetSummaryDatas()
        {
            SummaryDataWsResponce summaryDataWsResponce = new SummaryDataWsResponce();
            try
            {

                var urlService = "http://localhost:55014/adressinfo/summarydata/";
                summaryDataWsResponce = DownloadSerializedJsonData<SummaryDataWsResponce>(urlService);
            }
            catch (Exception ex)
            {
                summaryDataWsResponce.BaseResponse = new BaseResponse { ErorInfo = ErorCode.Error, MessageInfo = "Error Process!. Detail : " + ex.ToString() };
            }
            return summaryDataWsResponce;

        }

        public T DownloadSerializedJsonData<T>(string url) where T : new()
        {

            using (var w = new WebClient())
            {
                var json_data = string.Empty;
                json_data = w.DownloadString(url);

                return !string.IsNullOrEmpty(json_data) ? JsonConvert.DeserializeObject<T>(json_data) : new T();
            }
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
