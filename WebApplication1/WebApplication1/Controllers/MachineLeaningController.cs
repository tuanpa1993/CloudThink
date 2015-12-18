using System;
using System.Collections.Generic;
using System.Linq;
using System.IO;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using StatDescriptive;
using NUnit.Framework;
using Accord.Statistics;
using CenterSpace.NMath.Core;
using WebApplication1.Fus;
using MathWorks.MATLAB.NET; 
using MathWorks.MATLAB.NET.ComponentData;
using WebApplication1.Models;
using System.Net.Http.Formatting;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using Newtonsoft.Json.Linq;
using Newtonsoft.Json;

namespace WebApplication1.Controllers
{
    public class MachineLeaningController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get([FromUri]CloudData cloud)
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
                                                            "Anhnph"));
            DateTime inputTime = DateTime.Parse("12/16/2015");
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
            Random rnd = new Random();
            for (int i = 0; i < 10; i++)
            {
       

                CharterResult cha = new CharterResult()
                {
                    Intention = rnd.Next(1, 1000),
                    NonIntention = rnd.Next(1, 1000),
                    Hour = 10 + i,
                };
                listChart.Add(cha);                          
            }
            string json = JsonConvert.SerializeObject(new { CharterData = listChart });
            return Ok(new { CharterData = listChart });
        }
        [HttpPost]
        public IHttpActionResult Post(CloudData cloud)
        {


            List<Double> listData = new List<double>();
          
            string data = cloud.Data;
            OutputData resuls = new OutputData();
            data = data.Replace("[", "");
            data = data.Replace("]", "");
            data = data.Replace(" ", "");
            string[] split_datas = data.Split(',');
             
            foreach (string split_data in split_datas)
            {
                listData.Add(double.Parse(split_data));
            }

            InsertTable testFun = new InsertTable();
            resuls = testFun.GetML(listData, cloud);
            return Ok(resuls);

        }
    }
}
