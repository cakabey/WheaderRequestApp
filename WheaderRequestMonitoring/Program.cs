using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using ConsoleTables;
using Microsoft.AspNetCore.SignalR.Client;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;

namespace WheaderRequestMonitoring
{
    class Program
    {
        private static HubConnection _hubConnection;

        static void Main(string[] args)
        {
            GetData();
            MainAsync().GetAwaiter().GetResult();
        }
        private static void GetData()
        {
            SummaryDataWsResponce summaryDataWsResponce = ClientOperation.Instance.GetSummaryDatas();
            if (summaryDataWsResponce.BaseResponse.ErorInfo == ErorCode.Success)
            {
                List<SummaryDataTable> SummaryDataTable = new List<SummaryDataTable>();
                foreach (SummaryData summaryData in summaryDataWsResponce.SummaryData)
                {
                    SummaryDataTable summaryDataTable = new SummaryDataTable();
                    summaryDataTable.Temperature = summaryData.Temperature;
                    summaryDataTable.PlaceName = summaryData.PlaceName;
                    summaryDataTable.WeeklyMaxTemperature = summaryData.WeeklyMaxTemperature;
                    summaryDataTable.WeeklyMinTemperature = summaryData.WeeklyMinTemperature;
                    SummaryDataTable.Add(summaryDataTable);
                }
                Console.WriteLine("");
                Console.WriteLine("Daily web service requests. Date:" + DateTime.Now.ToString());
                Console.WriteLine("");
                ConsoleTable.From<SummaryDataTable>(SummaryDataTable).Write(Format.Minimal);
            }
            else
                Console.WriteLine(summaryDataWsResponce.BaseResponse.MessageInfo);

        }

        static async Task MainAsync()
        {
            try
            {
                await SetupSignalRHubAsync();
                Console.ReadLine();
            }
            catch (Exception)
            {
                await _hubConnection.DisposeAsync();
            }
        }
        private static async Task SetupSignalRHubAsync()
        {
            try
            {
                _hubConnection = new HubConnectionBuilder()
                    .WithUrl("http://localhost:55014/eventHub")
                   .Build();

                await _hubConnection.StartAsync();

                System.Console.WriteLine("Connected to Hub");
                System.Console.WriteLine("Press ESC to stop");


                _hubConnection.On<string>("SendNoticeEventToClient", (message) =>
                {
                    System.Console.WriteLine("ReceiveMessage to ");
                    if (message != null)
                    {

                        NewRequestData newRequestData = JsonConvert.DeserializeObject<NewRequestData>(message);

                        System.Console.WriteLine("\n Past Time(Seconds) : {0}", newRequestData.PastTime.ToString());
                  

                        System.Console.WriteLine("Request : {0}", newRequestData.Input);

                        System.Console.WriteLine("\nResponse :\n");
                        foreach (PropertyDescriptor descriptor in TypeDescriptor.GetProperties(newRequestData.SummaryData))
                        {
                            string nameX = descriptor.Name;
                            object value = descriptor.GetValue(newRequestData.SummaryData);
                            Console.WriteLine("   {0} = {1}", nameX, value);
                        }
                        System.Console.WriteLine("\n");
                    }
                });
            }
            catch (Exception ex)
            {

                Console.WriteLine(ex.ToString());
            }

        }



    }

}
