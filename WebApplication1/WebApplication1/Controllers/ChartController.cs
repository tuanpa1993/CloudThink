﻿using System;
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
        [HttpGet]
        public IHttpActionResult Get([FromUri]CloudData cloud)
        {
    
            return Ok("You do not have permission to view this directory or page.");
        }
        [HttpPost]
        public IHttpActionResult Post(ChartData chartdata)
        {
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
            int checkLastTime = 0;
            int lasttimeCheck = table.ExecuteQuery(query).Count();
            DateTime lastTime = new DateTime();
            foreach (CharterData entity in table.ExecuteQuery(query).OrderBy(b => b.Timestamp))
            {
                int checkdate = (inputTime - entity.Timestamp.Date).Days;
                checkLastTime = checkLastTime + 1;
                if (checkdate == 0)
                {
                    int day = entity.Timestamp.Hour;
                    if (checkDay != day && checkdate == 0)
                    {
                        if (listIstention.Count == 0 && listNonIstention.Count == 0)
                        {
                            checkDay = day;
                            listIstention.Clear();
                            listNonIstention.Clear();
                        }
                        else
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
                            checkDay = day;
                        }
                    }
                    else if (checkDay != day && checkdate != 0)
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
                        checkDay = day;
                    }
                    if (entity.IsIntention == 1) listIstention.Add(1);
                    else listNonIstention.Add(0);
                }
                if (checkLastTime == lasttimeCheck)
                {
                    lastTime = entity.Timestamp.DateTime;

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
            CharterResult ch2 = new CharterResult()
            {
                Intention = listIstention.Count,
                NonIntention = listNonIstention.Count,
                Hour = checkDay + 1,
                LastTime = lastTime,
            };
            listChart.Add(ch2);
            listIstention.Clear();
            listNonIstention.Clear();
            string json = JsonConvert.SerializeObject(new { CharterData = listChart });
            return Ok(new { CharterData = listChart });
        
        }
    }
}
