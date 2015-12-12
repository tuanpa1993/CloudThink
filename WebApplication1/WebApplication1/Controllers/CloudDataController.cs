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
using StatDescriptive;
using NUnit.Framework;
using Accord.Statistics;
using CenterSpace.NMath.Core;
using WebApplication1.Fus;
using vget_dataprocess;
using MathWorks.MATLAB.NET.Arrays;
using MathWorks.MATLAB.NET.Utility;
using MathWorks.MATLAB.NET.ComponentData;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{ 
    
    public class CloudDataController : ApiController
    {
        

        [HttpGet]
        public IHttpActionResult Get([FromUri]CloudData cloud) 
        {

            string accountName = "cloudthink";
            string accountKey = "hVkTDM6KYgHZF1X8buidcV2Uwam1UTczsdZ9M2OIaMNkN12rR++mRZgzMH401dYcKBg0/3cRSX6EzAa6IXMW3g==";
            StorageCredentials creds = new StorageCredentials(accountName, accountKey);
            CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
            CloudTableClient client = account.CreateCloudTableClient();
            CloudTable table = client.GetTableReference("cloudthinktest");
            table.CreateIfNotExists();
            DateTime nowtime = DateTime.Parse("12/12/2015 12:00:00 AM");
            TableQuery<CharterData> query = new TableQuery<CharterData>()
                   .Where(TableQuery.GenerateFilterCondition("Name",
                                                             QueryComparisons.Equal,
                                                            "Anhnph"));
            List<int> listIstention = new List<int>();
            List<int> listNonIstention = new List<int>();
            foreach (CharterData entity in table.ExecuteQuery(query))
            {
                DateTime un = entity.Timestamp.Date;
                string abc = entity.Gender;
                string bc = entity.IsIntention;
                int checkDuration = (nowtime - un).Days; 
                if (checkDuration == 1)
                {
                    if (entity.IsIntention.Equals("1")) listIstention.Add(1);
                    else listNonIstention.Add(0);
                }                                              
            }
            float percent =   (listIstention.Count + listNonIstention.Count);

            percent = (listIstention.Count / percent)*100;
            return Ok(percent);
            
        }  
        [HttpPost]
        public IHttpActionResult Post(CloudData cloud)
        {


            string accountName = "cloudthink";
            string accountKey = "hVkTDM6KYgHZF1X8buidcV2Uwam1UTczsdZ9M2OIaMNkN12rR++mRZgzMH401dYcKBg0/3cRSX6EzAa6IXMW3g==";
            try
            {
                StorageCredentials creds = new StorageCredentials(accountName, accountKey);
                CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
                CloudTableClient client = account.CreateCloudTableClient();
                CloudTable table = client.GetTableReference("cloudthink");
                TableBatchOperation batchOperation = new TableBatchOperation();
                table.CreateIfNotExists();
                List<Double> listData = new List<double>();
                string data = cloud.Data;
                PointData entity = new PointData();
                OutputData resuls = new OutputData();
                data = data.Replace("[", "");
                data = data.Replace("]", "");
                data = data.Replace(" ", "");
                string[] split_datas = data.Split(',');
                foreach (string split_data in split_datas)
                {
                    listData.Add(double.Parse(split_data));
                }

                int l = 0;
                int overlap = 15;
                int dem = 1;
                int check = 0;
                int removeCount = listData.Count() - 1023;
                if (listData.Count() > 2050)
                {
                    listData.RemoveRange(removeCount, 1023);
                    listData.RemoveRange(0, 1023);
                }
                double[] test = listData.ToArray();
                int div = test.Length / 512;
                int sqv = div * 512;
                if (sqv < test.Length) div = div + 1;
                List<Double> dl = new List<double>();
                int lap = test.Length / 16;
                for (int i = 0; i < test.Length; i++)
                {
                    if (i == 0)
                    {
                        dl.Clear();
                        int loop = i + 512;
                        for (int j = i; j < loop; j++)
                        {
                            dl.Add(test[j]);
                        }



                        InsertTable testFun = new InsertTable();
                        entity = testFun.dataCloud(dl, cloud);
                        batchOperation.Insert(entity);

                        //TableOperation inser = TableOperation.Insert(entity);
                        //table.Execute(inser);
                        //testFun.InsertData(dl, cloud);
                    }
                    else if (i == overlap && dem <= lap)
                    {
                        dl.Clear();
                        int loop = i + 512;
                        if (loop <= test.Length - 1)
                        {
                            for (int j = i; j < loop; j++)
                            {
                                dl.Add(test[j]);
                            }
                            InsertTable testFun = new InsertTable();

                            entity = testFun.dataCloud(dl, cloud);
                            // TableOperation inser = TableOperation.Insert(entity);
                            batchOperation.Insert(entity);
                            //table.Execute(inser);
                            //testFun.InsertData(dl, cloud);
                        }
                        else
                        {

                            dem = lap + 1;
                        }
                        overlap = overlap + 16;
                        dem = dem + 1;
                    }
 
                    if (batchOperation.Count() == 98)
                    {
                        table.ExecuteBatch(batchOperation);
                        batchOperation.Clear();
                        // batchOperation.Clear();
                        //table.ExecuteBatch(batchOperation);
                        // demCheck = 0;
                    }
                }

                table.ExecuteBatch(batchOperation);
            }
            catch (Exception ex)
            {
                //  ex.
            }
            return Ok("success");

        }
   
         
    }

    
}

