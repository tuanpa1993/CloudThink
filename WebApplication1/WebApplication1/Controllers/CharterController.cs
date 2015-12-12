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
    public class CharterController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post(ChartData chartdata)
        {
            string accountName = "cloudthink";
            string accountKey = "hVkTDM6KYgHZF1X8buidcV2Uwam1UTczsdZ9M2OIaMNkN12rR++mRZgzMH401dYcKBg0/3cRSX6EzAa6IXMW3g==";
            StorageCredentials creds = new StorageCredentials(accountName, accountKey);
            CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
            CloudTableClient client = account.CreateCloudTableClient();
            CloudTable table = client.GetTableReference("cloudthinktest");
            table.CreateIfNotExists();
            DateTime nowtime = chartdata.TimeChart.Date;
            TableQuery<CharterData> query = new TableQuery<CharterData>()
                   .Where(TableQuery.GenerateFilterCondition("Name",
                                                             QueryComparisons.Equal,
                                                            chartdata.Name));
            List<int> listIstention = new List<int>();
            List<int> listNonIstention = new List<int>();
            List<CharterResult> listChart = new List<CharterResult>();
            int checkLoop = 0;
            try
            {
                foreach (CharterData entity in table.ExecuteQuery(query))
                {
                    DateTime un = entity.Timestamp.Date;
                    int checkDuration = (nowtime - un).Days;
                    if (checkLoop != checkDuration && checkDuration >0)
                    {
                        if (checkLoop == 0)
                        {
                            checkLoop = checkDuration;
                        }
                        else
                        {
                            CharterResult ch = new CharterResult()
                            {
                                Intention = listIstention.Count,
                                NonIntention = listNonIstention.Count,
                                TimeChart = un,
                            };
                            listIstention.Clear();
                            listNonIstention.Clear();
                            checkLoop = checkDuration;
                        }

                    }
                    if (checkDuration >= 0 && checkDuration <= chartdata.Duration)
                    {
                        if (entity.IsIntention.Equals("1")) listIstention.Add(1);
                        else listNonIstention.Add(0);
                    }
                }
            }
            catch(Exception ex)
            {
                return Ok("error");
            }
                
                string json = JsonConvert.SerializeObject(new { CharterData = listChart });
                return Ok(json);
        }
    }
}
