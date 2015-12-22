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
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    public class CollectDataController : ApiController
    {
        public IHttpActionResult Post(CloudData cloud)
        {


            string accountName = "cloudthinkstorage";
            string accountKey = "u0gOpUjoxc9OWpjSBTSvm7tFHZPz8r8iFKK4uWxjtnC3Sh17oKytYMxR69lsfmGfkcmoQNmPPRWbD12l8QyNbg==";
            try
            {
                StorageCredentials creds = new StorageCredentials(accountName, accountKey);
                CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
                CloudTableClient client = account.CreateCloudTableClient();
                CloudTable table = client.GetTableReference("cloudthinktest");
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
