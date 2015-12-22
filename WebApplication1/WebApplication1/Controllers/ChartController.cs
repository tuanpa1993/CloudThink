using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    public class ChartController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post(ChartData chartdata)
        {
            //string accountName = "cloudthinkstorage";
            //string accountKey = "u0gOpUjoxc9OWpjSBTSvm7tFHZPz8r8iFKK4uWxjtnC3Sh17oKytYMxR69lsfmGfkcmoQNmPPRWbD12l8QyNbg==";
            //StorageCredentials creds = new StorageCredentials(accountName, accountKey);
            //CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
            //CloudTableClient client = account.CreateCloudTableClient();
            //CloudTable table = client.GetTableReference("cloudthinktest");
            //table.CreateIfNotExists();
            //TableQuery<CharterData> query = new TableQuery<CharterData>()
            //       .Where(TableQuery.GenerateFilterCondition("Name",
            //                                                 QueryComparisons.Equal,
            //                                                chartdata.Name));
            //int typeDate = chartdata.TypeDate;
            //DateTime inputTime = chartdata.TimeChart.Date;
            //List<int> listIstention = new List<int>();
            //List<int> listNonIstention = new List<int>();
            //List<CharterResult> listChart = new List<CharterResult>();
            //if (typeDate == 0)
            //{
            //    try
            //    {
            //        foreach (CharterData entity in table.ExecuteQuery(query))
            //        {
            //            DateTime timeQuery = entity.Timestamp.Date;
            //            int checkDuration = (inputTime - timeQuery).Days;
            //            if (checkDuration == 0)
            //            {
            //                if (entity.IsIntention.Equals("1")) listIstention.Add(1);
            //                else listNonIstention.Add(0);
            //            }
            //        }
            //        CharterResult ch = new CharterResult()
            //        {
            //            Intention = listIstention.Count,
            //            NonIntention = listNonIstention.Count,
            //            TimeChart = inputTime,
            //        };
            //        listChart.Add(ch);
            //        listIstention.Clear();
            //        listNonIstention.Clear();
            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
            //else if (typeDate == 1)
            //{
            //    try
            //    {
            //        DateTime firstDayOfMonth = inputTime;
            //        DateTime lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);
            //        int durationDate = (lastDayOfMonth - firstDayOfMonth).Days;
            //        for (int i = 0; i <= durationDate; i++)
            //        {
            //            foreach (CharterData entity in table.ExecuteQuery(query))
            //            {
            //                DateTime CheckTime = entity.Timestamp.Date;
            //                int checkDuration = (CheckTime - firstDayOfMonth).Days;
            //                if (checkDuration == i)
            //                {
            //                    if (entity.IsIntention.Equals("1")) listIstention.Add(1);
            //                    else listNonIstention.Add(0);

            //                }
            //            }
            //            CharterResult ch = new CharterResult()
            //            {
            //                Intention = listIstention.Count,
            //                NonIntention = listNonIstention.Count,
            //                TimeChart = firstDayOfMonth.AddDays(i),
            //            };
            //            listChart.Add(ch);
            //            listIstention.Clear();
            //            listNonIstention.Clear();

            //        }

            //    }
            //    catch (Exception ex)
            //    {

            //    }
            //}
           // string json = JsonConvert.SerializeObject(new { CharterData = listChart });
            string accountName = "cloudthinkstorage";
            string accountKey = "u0gOpUjoxc9OWpjSBTSvm7tFHZPz8r8iFKK4uWxjtnC3Sh17oKytYMxR69lsfmGfkcmoQNmPPRWbD12l8QyNbg==";
            StorageCredentials creds = new StorageCredentials(accountName, accountKey);
            CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
            CloudTableClient client = account.CreateCloudTableClient();
            CloudTable table = client.GetTableReference("cloudthinktest");
            table.CreateIfNotExists();
            TableQuery<CharterData> query = new TableQuery<CharterData>()
                   .Where(TableQuery.GenerateFilterCondition("Name",
                                                             QueryComparisons.Equal,
                                                            chartdata.Name));
            DateTime inputTime = chartdata.TimeChart;
            List<int> listIstention = new List<int>();
            List<int> listNonIstention = new List<int>();
            List<CharterResult> listChart = new List<CharterResult>();
            int checkDay = 0;
            int checkAdd = 0;
            foreach (CharterData entity in table.ExecuteQuery(query))
            {
                int checkdate = (inputTime - entity.Timestamp.Date).Days;
                if (checkdate == 0)
                {
                    int day = entity.Timestamp.Hour;
                    if (checkDay != day)
                    {
                        if (listIstention.Count == 0 && listNonIstention.Count == 0)
                        {
                            checkDay = day;
                            listIstention.Clear();
                            listNonIstention.Clear();
                        }
                        else
                        {
                            checkDay = day;
                            CharterResult ch = new CharterResult()
                            {
                                Intention = listIstention.Count,
                                NonIntention = listNonIstention.Count,
                                Hour = checkDay + 1,
                            };
                            listChart.Add(ch);
                            listIstention.Clear();
                            listNonIstention.Clear();
                        }
                    }
                    if (entity.IsIntention==1) listIstention.Add(1);
                    else listNonIstention.Add(0);
                }
                else if (checkdate == -1)
                {
                    if (checkAdd == 0)
                    {
                        CharterResult ch = new CharterResult()
                        {
                            Intention = listIstention.Count,
                            NonIntention = listNonIstention.Count,
                            Hour = checkDay + 1,
                        };
                        listChart.Add(ch);
                        listIstention.Clear();
                        listNonIstention.Clear();
                        checkAdd = 1;
                    }

                }
            }
            if (checkAdd == 0)
            {
                CharterResult ch1 = new CharterResult()
                {
                    Intention = listIstention.Count,
                    NonIntention = listNonIstention.Count,
                    Hour = checkDay + 1,
                };
                listChart.Add(ch1);
                listIstention.Clear();
                listNonIstention.Clear();
             
            }
            string json = JsonConvert.SerializeObject(new { CharterData = listChart });
            return Ok(new { CharterData = listChart });
        
        }
    }
}
