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
    public class ProfileController : ApiController
    {
        [HttpPost]
        public IHttpActionResult Post(string UserName)
        {
            string checkUserName ="false";
            string accountName = "dataprocess";
            string accountKey = "3jRL8UwYbjGMO0x49dcbEWdjZLJmHxAJGEydeGlh+u/5qX693pVYPl2Q9FdaEyZrALcLid8YLn4Sz5uW+x56Ng==";
            List<String> dl = new List<string>();
            StorageCredentials creds = new StorageCredentials(accountName, accountKey);
            CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
            CloudTableClient client = account.CreateCloudTableClient();
            CloudTable table = client.GetTableReference("MyAdmin");
            table.CreateIfNotExists();
            TableQuery<Profile> query = new TableQuery<Profile>()
                    .Where(TableQuery.GenerateFilterCondition("UserName",
                                                              QueryComparisons.Equal,
                                                              UserName));
            foreach (Profile entity in table.ExecuteQuery(query))
            {
                string un = entity.UserName;
                if (un == null)
                {
                    checkUserName = "false";
                }
                else
                {
                    checkUserName = "true";
                }
                

            }
            return Ok(checkUserName);
        }
        [HttpPost]
        public IHttpActionResult Post([FromUri]Profile profile)
        {
            try
            {
                string accountName = "dataprocess";
                string accountKey = "3jRL8UwYbjGMO0x49dcbEWdjZLJmHxAJGEydeGlh+u/5qX693pVYPl2Q9FdaEyZrALcLid8YLn4Sz5uW+x56Ng==";
                StorageCredentials creds = new StorageCredentials(accountName, accountKey);
                CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
                CloudTableClient client = account.CreateCloudTableClient();
                CloudTable table = client.GetTableReference("MyAdmin");
                table.CreateIfNotExists();
                Random rd = new Random();
                Profile entity = new Profile("123", "" + rd.Next(1, 100000000))
                {
                    Email = profile.Email,
                    PassWord = profile.PassWord,
                    UserName = profile.UserName,
                    Age = profile.Age,
                    Gender = profile.Gender
                };
                TableOperation inser = TableOperation.Insert(entity);
                table.Execute(inser);
            }
            catch (Exception ex) { 
}
            return Ok(profile);
        }
    }
}
