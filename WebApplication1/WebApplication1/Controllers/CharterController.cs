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
        [HttpGet]
        public IHttpActionResult Get(string username)
        {
            string accountName = "cloudthink";
            string accountKey = "1F/oKHxUjXl6Ox1D9fM74htSX+zugkYKmL2MjprPczZAhOeGgPcPBudIypvwkPRfTYypeho8/bksBapafBsFOw==";
            StorageCredentials creds = new StorageCredentials(accountName, accountKey);
            CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
            CloudTableClient client = account.CreateCloudTableClient();
            CloudTable table = client.GetTableReference("cloudthink");
            // table.CreateIfNotExists();
            //string lowerlimit = DateTime.Today.AddDays(-52).ToString("yyyy-MM-dd");

            //string dateRangeFilter = TableQuery.CombineFilters(
            //    TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, "dataprocess"),
            //    TableOperators.And,
            //    TableQuery.GenerateFilterConditionForDate("TimeStamp", QueryComparisons.GreaterThanOrEqual, DateTime.Today.AddDays(-52)));
            //   TableQuery<DynamicTableEntity> projectionQuery = new TableQuery<DynamicTableEntity>().Select(new string[] { "IsIntention" });
            //     EntityResolver<string> resolver = (pk, rk, ts, props, etag) => props.ContainsKey("IsIntention") ? props["IsIntention"].StringValue : null;
            List<string> listData = new List<string>();
            //   foreach (string projectedEmail in table.ExecuteQuery(projectionQuery, resolver, null, null))
            //   {
            //         listData.Add(projectedEmail);
            //       Console.WriteLine(projectedEmail);
            //    }
            return Ok(listData);
        }
    }
}
