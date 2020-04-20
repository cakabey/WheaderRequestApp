
using Microsoft.AspNet.SignalR.Client;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using static WheaderRequest.SendNotif;

namespace WheaderRequest.Controllers
{

    [ApiController]
    [Route("[controller]")]
    public class AdressInfoController : ControllerBase
    {
        private readonly ILogger<AdressInfo> _logger;
        private IConfiguration _configuration;
        private readonly IMemoryCache memoryCache;
        private readonly IHubContext<EventHub> _eventHub;
        public AdressInfoController(ILogger<AdressInfo> logger, IConfiguration iConfig, IMemoryCache memoryCach, IHubContext<EventHub> eventHub)
        {
            _logger = logger;
            _configuration = iConfig;
            this.memoryCache = memoryCach;
            _eventHub = eventHub;
        }

        [HttpGet("request/{adress}")]

        public string Get(string adress)
        {
            AdressInfoWsResponce adressInfoWsResponce = new AdressInfoWsResponce();
            DateTime startTime = DateTime.Now;
            try
            {
                SummaryData summaryDataCache = new SummaryData();


                bool isExist = false;
                var field = typeof(MemoryCache).GetProperty("EntriesCollection", BindingFlags.NonPublic | BindingFlags.Instance);
                var collection = field.GetValue(memoryCache) as ICollection;
                if (collection.Count <= 50)
                    isExist = memoryCache.TryGetValue(adress, out summaryDataCache);

                if (isExist)
                {
                    adressInfoWsResponce.SummaryData = summaryDataCache;
                    adressInfoWsResponce.BaseResponse = new BaseResponse { ErorInfo = ErorCode.Success, MessageInfo = "Succes Process" };
                    return JsonConvert.SerializeObject(adressInfoWsResponce);
                }

                using (var db = new SQLiteDBContext())
                {
                    var process = db.WsProcessInfo
                          .Where(t => t.ProcessDate > DateTime.Now.AddHours(-DateTime.Now.Hour))
                          .Where(t => t.ProcessDate <= DateTime.Now.AddHours(24 - DateTime.Now.Hour))
                          .Where(t => t.AdressName == adress).OrderByDescending(t => t.Id);

                    if (process.Count() > 0)
                    {
                        WsProcessInfo wsProcessInfoDb = process.FirstOrDefault();

                        var query = db.SummaryData
                        .Where(t => t.ProcessId == wsProcessInfoDb.Id);

                        if (query.Count() > 0)
                        {
                            adressInfoWsResponce.SummaryData = query.FirstOrDefault();
                            adressInfoWsResponce.BaseResponse = new BaseResponse { ErorInfo = ErorCode.Success, MessageInfo = "Succes Process" };
                            return JsonConvert.SerializeObject(adressInfoWsResponce);
                        }
                    }
                }


                List<AdressInfo> adressInfos;
                string locationIqToken = _configuration.GetSection("WheaderRequestConfig").GetSection("LocationIqToken").Value;
                string darkSkyKey = _configuration.GetSection("WheaderRequestConfig").GetSection("DarkSkyKey").Value;

                WsProcessInfo wsProcessInfo = new WsProcessInfo
                {
                    ProcessDate = DateTime.Now,
                    ProcessName = "GetAdressInfoTempruter",
                    AdressName = adress

                };
                using (var db = new SQLiteDBContext())
                {
                    db.WsProcessInfo.Add(
                      wsProcessInfo
                        );
                    db.SaveChanges();
                }

                var url = "https://eu1.locationiq.com/v1/search.php?key=" + locationIqToken + "&q= " + adress + " &format=json";
                var currencyRates = Common.DownloadSerializedJsonData<List<AdressInfo>>(url);


                adressInfos = currencyRates;

                if (adressInfos.Count == 0)
                {
                    adressInfoWsResponce.BaseResponse = new BaseResponse { ErorInfo = ErorCode.Error, MessageInfo = "Error Process!. Detail : " + "Location Data Not Found!" };
                    return JsonConvert.SerializeObject(adressInfoWsResponce);
                }
                AdressInfo adressInfo = adressInfos[0];
                adressInfo.RequestInfoId = wsProcessInfo.Id;

                var urlSky = "https://api.darksky.net/forecast/" + darkSkyKey + "/" + adressInfo.Lat + "," + adressInfo.Lon;
                var wheaderWsInfo = Common.DownloadSerializedJsonData<WheaderWsInfo>(urlSky);

                if (wheaderWsInfo == null)
                {
                    adressInfoWsResponce.BaseResponse = new BaseResponse { ErorInfo = ErorCode.Error, MessageInfo = "Error Process!. Detail : " + "Temperature Data Not Found!" };
                    return JsonConvert.SerializeObject(adressInfoWsResponce);
                }

                Datum datum = wheaderWsInfo.Daily.Data[0];//currentDate
                Daily daily = wheaderWsInfo.Daily;//weekly


                SummaryData summaryData = new SummaryData();
                summaryData.DayDate = DateTime.Now;
                summaryData.ProcessId = wsProcessInfo.Id;
                summaryData.PlaceName = adressInfo.DisplayName;
                summaryData.PlaceId = adressInfo.PlaceId;
                summaryData.DailyMinTemperature = datum.TemperatureMin.ToString();
                summaryData.DailyMaxTemperature = datum.TemperatureMax.ToString();
                summaryData.WeeklyMinTemperature = Common.WeeklyMinTempruter(daily.Data).ToString();
                summaryData.WeeklyMaxTemperature = Common.WeeklyMaxTempruter(daily.Data).ToString();
                summaryData.Lat = wheaderWsInfo.Latitude.ToString();
                summaryData.Lon = wheaderWsInfo.Longitude.ToString();
                summaryData.Temperature = wheaderWsInfo.Currently.Temperature.ToString();
                using (var db = new SQLiteDBContext())
                {
                    db.SummaryData.Add(summaryData);
                    db.SaveChanges();
                }

                if (!isExist && collection.Count < 50)
                {
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
            .SetSlidingExpiration(TimeSpan.FromSeconds(30));
                    memoryCache.Set(adress, summaryData, cacheEntryOptions);


                    DateTime finishTime = DateTime.Now;
                    TimeSpan resultSpan = finishTime - startTime;

                    NewRequestData newRequestData = new NewRequestData();

                    newRequestData.SummaryData = summaryData;
                    newRequestData.PastTime = resultSpan.TotalSeconds;
                    newRequestData.Input = adress;
                    _eventHub.Clients.All.SendAsync("SendNoticeEventToClient",
             JsonConvert.SerializeObject(newRequestData));

                }
                adressInfoWsResponce.SummaryData = summaryData;
                adressInfoWsResponce.BaseResponse = new BaseResponse { ErorInfo = ErorCode.Success, MessageInfo = "Succes Process" };
            }
            catch (Exception ex)
            {

                adressInfoWsResponce.BaseResponse = new BaseResponse { ErorInfo = ErorCode.Error, MessageInfo = "Error Process!. Detail " + ex.Message };
            }

            return JsonConvert.SerializeObject(adressInfoWsResponce);


        }

        [HttpGet("summarydata")]

        public string GetSummary()
        {
            SummaryDataWsResponce summaryDataWsResponce = new SummaryDataWsResponce();
            try
            {
                using (var db = new SQLiteDBContext())
                {
                    var query = db.SummaryData
                   .Where(t => t.DayDate >= DateTime.Now.Date)
                          .Where(t => t.DayDate < DateTime.Now.AddHours(24 - DateTime.Now.Hour));
                    summaryDataWsResponce.SummaryData = query.ToArray();
                }
                summaryDataWsResponce.BaseResponse = new BaseResponse { ErorInfo = ErorCode.Success, MessageInfo = "Succes Process" };
            }
            catch (Exception ex)
            {
                summaryDataWsResponce.BaseResponse = new BaseResponse { ErorInfo = ErorCode.Error, MessageInfo = "Error Process!. Detail " + ex.Message };
            }

            return JsonConvert.SerializeObject(summaryDataWsResponce);
        }


    }
}