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
            foreach (CharterData entity in table.ExecuteQuery(query))
            {
                DateTime un = entity.Timestamp.Date;
                string abc = entity.Gender;
                string bc = entity.IsIntention;
                int checkDuration = (nowtime - un).Days;
                if (checkDuration == chartdata.Duration)
                {
                    if (entity.IsIntention.Equals("1")) listIstention.Add(1);
                    else listNonIstention.Add(0);
                }
            }
            float percent = (listIstention.Count + listNonIstention.Count);

            percent = (listIstention.Count / percent) * 100;
            return Ok(percent);
        }
    }
}
