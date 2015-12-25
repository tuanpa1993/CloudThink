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
            PointData entity = new PointData();
            List<Double> listData = new List<double>();
            string data = "[503.0, 536.0, 500.0, 468.0, 478.0, 477.0, 425.0, 509.0, 434.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 468.0, 384.0, 447.0, 425.0, 509.0, 434.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 484.0, 448.0, 430.0, 425.0, 509.0, 434.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 421.0, 461.0, 425.0, 509.0, 434.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 444.0, 445.0, 424.0, 421.0, 398.0, 390.0, 408.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 390.0, 338.0, 340.0, 387.0, 361.0, 384.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 380.0, 342.0, 426.0, 426.0, 375.0, 379.0, 389.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 342.0, 426.0, 426.0, 375.0, 379.0, 389.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 327.0, 405.0, 417.0, 426.0, 375.0, 379.0, 389.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 353.0, 352.0, 375.0, 379.0, 389.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 387.0, 397.0, 399.0, 387.0, 375.0, 395.0, 395.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 397.0, 399.0, 387.0, 375.0, 395.0, 395.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 431.0, 427.0, 405.0, 375.0, 395.0, 395.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 408.0, 396.0, 378.0, 395.0, 395.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 409.0, 411.0, 375.0, 395.0, 395.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 406.0, 383.0, 375.0, 395.0, 395.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 398.0, 418.0, 368.0, 375.0, 395.0, 395.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 421.0, 436.0, 428.0, 395.0, 395.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 488.0, 527.0, 375.0, 395.0, 395.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 533.0, 574.0, 581.0, 607.0, 375.0, 395.0, 395.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 611.0, 645.0, 625.0, 375.0, 395.0, 395.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 625.0, 610.0, 644.0, 375.0, 395.0, 395.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 661.0, 670.0, 375.0, 395.0, 395.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 623.0, 665.0, 716.0, 375.0, 395.0, 395.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 614.0, 624.0, 670.0, 375.0, 395.0, 395.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 618.0, 593.0, 593.0, 375.0, 395.0, 395.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0, 590.0, 550.0, 549.0, 395.0, 395.0, 575.0, 581.0, 524.0, 507.0, 552.0, 562.0, 486.0, 481.0, 514.0, 583.0, 509.0, 503.0, 536.0]";
            OutputData resuls = new OutputData();
            InsertTable testFun = new InsertTable();
            data = data.Replace("[", "");
            data = data.Replace("]", "");
            data = data.Replace(" ", "");
            string[] split_datas = data.Split(',');

            foreach (string split_data in split_datas)
            {
                listData.Add(double.Parse(split_data));
            }
            resuls = testFun.GetML(listData, cloud);

            testFun.InsertData(listData, cloud, resuls.Score);
            return Ok("success");

        }  
        public IHttpActionResult Post(CloudData cloud)
        {
            string accountName = "cloudthinkstorage";
            string accountKey = "u0gOpUjoxc9OWpjSBTSvm7tFHZPz8r8iFKK4uWxjtnC3Sh17oKytYMxR69lsfmGfkcmoQNmPPRWbD12l8QyNbg==";
            StorageCredentials creds = new StorageCredentials(accountName, accountKey);
            CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
            CloudTableClient client = account.CreateCloudTableClient();
            CloudTable table = client.GetTableReference("cloudthinktest");
            table.CreateIfNotExists();
            PointData entity = new PointData();
            List<Double> listData = new List<double>();
            string data = cloud.Data;
            OutputData resuls = new OutputData();
            InsertTable testFun = new InsertTable();
            data = data.Replace("[", "");
            data = data.Replace("]", "");
            data = data.Replace(" ", "");
            string[] split_datas = data.Split(',');

            foreach (string split_data in split_datas)
            {
                listData.Add(double.Parse(split_data));
            }
            resuls = testFun.GetML(listData, cloud);
           
                testFun.InsertData(listData, cloud, resuls.Score);

         
            
            return Ok(resuls.Score);

        }
    }
}
