using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using StatDescriptive;
using NUnit.Framework;
using Accord.Statistics;
using CenterSpace.NMath.Core;
using WebApplication1.Fus;
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
using System.Text; 

namespace WebApplication1.Fus
{
    public class InsertTable
    {
        public static string dataResultIStention ;
        public static string dataResultScore;
        public static string dataTest;
         public class StringTable
        {
            public string[] ColumnNames { get; set; }
            public string[,] Values { get; set; }
        }
         static async Task InvokeRequestResponseService(string Age, string Name, string Gender, string IsIntention, string Kurtosis, string Skewness, string secondDiffisNormalize, string firstDiffisNormalize, string percent_alpha, string percent_beta, string psd_alpha, string psd_beta, string arburg_1, string arburg_2, string arburg_3, string arburg_4, string arburg_5, string arburg_6)
         {
         
             using (var client = new HttpClient())
             {
                 var scoreRequest = new
                 {

                     Inputs = new Dictionary<string, StringTable>() { 
                        { 
                            "input1", 
                            new StringTable() 
                            {
                                ColumnNames = new string[] {"PartitionKey", "RowKey", "Timestamp", "Age", "Name", "Gender", "IsIntention", "Kurtosis", "Skewness", "secondDiffisNormalize", "firstDiffisNormalize", "percent_alpha", "percent_beta", "psd_alpha", "psd_beta","arburg_1","arburg_2","arburg_3","arburg_4","arburg_5","arburg_6"},
                                Values = new string[,] {  {"value", "value", "",  Age, Name, Gender,IsIntention, Kurtosis, Skewness, secondDiffisNormalize, firstDiffisNormalize, percent_alpha, percent_beta, psd_alpha,psd_beta,arburg_1,arburg_2,arburg_3,arburg_4,arburg_5,arburg_6 },  {"value", "value", "",  Age, Name, Gender,IsIntention, Kurtosis, Skewness, secondDiffisNormalize, firstDiffisNormalize, percent_alpha, percent_beta, psd_alpha,psd_beta ,arburg_1,arburg_2,arburg_3,arburg_4,arburg_5,arburg_6},  }
                            }
                        },
                                        },
                     GlobalParameters = new Dictionary<string, string>()
                     {
                     }
                 };
                 const string apiKey = "vRqr2moZYGn5XBv8E8Ap+X7qfb38gLf9nsp+EoUjEZNaf1WgFifGXxlVE0DcNNyHT7AhYj4s7uVZyecWi92Zkw=="; // Replace this with the API key for the web service
                 client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

                 client.BaseAddress = new Uri("https://asiasoutheast.services.azureml.net/workspaces/4301cc0e7cb149dcb5812642e453d8f7/services/a99bffd83ab34bdaad98d130e8ec9643/execute?api-version=2.0&details=true");

                 // WARNING: The 'await' statement below can result in a deadlock if you are calling this code from the UI thread of an ASP.Net application.
                 // One way to address this would be to call ConfigureAwait(false) so that the execution does not attempt to resume on the original context.
                 // For instance, replace code such as:
                 //      result = await DoSomeTask()
                 // with the following:
                 //      result = await DoSomeTask().ConfigureAwait(false)


                 HttpResponseMessage response = await client.PostAsJsonAsync("", scoreRequest).ConfigureAwait(false);

                 if (response.IsSuccessStatusCode)
                 {
                     string result = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                     dataTest = result;
                     List<OutputObject> listOutputObject = ExtractValuesObject(result);               
                      //   dataResult = result;
                     foreach (OutputObject outputObj in listOutputObject)
                     {
                         
                         for (int i = 0; i < outputObj.Values.Count; i++)
                         {

                             dataResultIStention = outputObj.Values[2];
                             dataResultScore = outputObj.Values[19];
                            // dataResult = outputObj.Name;
                          //   dataResult = outputObj.Values;                          
                             
                         }
                         
                     }
                     // Console.WriteLine("Result: {0}", result);
                 }
                 else
                 {
                     //Console.WriteLine(string.Format("The request failed with status code: {0}", response.StatusCode));

                     //// Print the headers - they include the requert ID and the timestamp, which are useful for debugging the failure
                     //Console.WriteLine(response.Headers.ToString());

                     string responseContent = await response.Content.ReadAsStringAsync();
                     dataResultIStention = responseContent;
                     //Console.WriteLine(responseContent);
                 }
             }
              
         }
         static List<OutputObject> ExtractValuesObject(string jsonStr)
         {
             try
             {
                 List<OutputObject> listOutput = new List<OutputObject>();
                 var objects = JObject.Parse(JObject.Parse(jsonStr)["Results"].ToString());

                 foreach (var output in objects)
                 {
                     OutputObject tmpOutput = new OutputObject();
                     tmpOutput.Name = output.Key;
                     try
                     {
                         JArray outputArray = JArray.Parse(output.Value["value"]["Values"][0].ToString());
                         foreach (var outputValue in outputArray)
                             tmpOutput.Values.Add(outputValue.ToString());
                     }
                     catch (Exception) { }
                     finally { listOutput.Add(tmpOutput); };
                 }
                 return listOutput;
             }
             catch (Exception)
             {
                 return null;
             }
         }

         public string GetMLTest(List<Double> data, CloudData cloud)
         {
             double[] test = data.ToArray();
             Descriptive desp = new Descriptive(test);
             desp.Analyze();
             LibHander output = new LibHander();
             string Age = "20";
             string Name = "HungNN2";
             string Gender = cloud.Gender + "F";
             string IsIntention = cloud.IsIntention + "";
             string Entropy = Tools.Entropy(test) + "";
             string StandardDeviation = Tools.StandardDeviation(test) + "";
             string Mean = Tools.Mean(test) + "";
             string Kurtosis = Tools.Kurtosis(test) + "";
             string Skewness = Tools.Skewness(test) + "";
             string Max = test.Max() + "";
             string StdDev = desp.Result.StdDev + "";
             string secondDiff = output.vget_mean_secondDiff(test, false) + "";
             string firstDiff = output.vget_mean_firstDiff(test, false) + "";
             string secondDiffisNormalize = output.vget_mean_secondDiff(test, true) + "";
             string firstDiffisNormalize = output.vget_mean_firstDiff(test, true) + "";
             try
             {
                // InvokeRequestResponseService(Age, Name, Gender, IsIntention, Entropy, StandardDeviation, Mean, Kurtosis, Skewness, Max, StdDev, secondDiff, firstDiff, secondDiffisNormalize, firstDiffisNormalize).Wait();

             }
             catch (Exception ex)
             {
                  
             }
             //OutputData outputdata = new OutputData()
             //{
             //    Istention = dataResultIStention,
             //    Score = dataResultScore,
             //};
             return dataTest;
         }
         public OutputData GetML(List<Double> data, CloudData cloud)
         {
             double[] test = data.ToArray();
             Descriptive desp = new Descriptive(test);
             desp.Analyze();
             LibHander output = new LibHander();
             double[] arburg = output.arburg(test, 6);
             FftEeg fe = new FftEeg();
             FftData fd = new FftData();
             fd = fe.Ffteeg(test, 512);
             string Age = cloud.Age+ "";
             string Name = cloud.Name + "";
             string Gender = cloud.Gender + "";
             string IsIntention = cloud.IsIntention + "";    
             string percent_alpha = output.vget_percent_frequency(fd, "alpha") + "";
             string percent_beta = output.vget_percent_frequency(fd, "beta") + "";
             string psd_alpha = output.vget_energy(test, 512, "alpha") + "";
             string psd_beta = output.vget_energy(test, 512, "beta") + "";
             string Kurtosis = Tools.Kurtosis(test) + "";
             string Skewness = Tools.Skewness(test) + "";         
             string secondDiff = output.vget_mean_secondDiff(test,false) + "";
             string firstDiff = output.vget_mean_firstDiff(test, false) + "";
             string secondDiffisNormalize = output.vget_mean_secondDiff(test,true) + "";
             string firstDiffisNormalize = output.vget_mean_firstDiff(test, true) + "";
             string arburg_1 = arburg[0] + "";
             string arburg_2 = arburg[1] + "";
             string arburg_3 = arburg[2] + "";
             string arburg_4 = arburg[3] + "";
             string arburg_5 = arburg[4] + "";
             string arburg_6 = arburg[5] + "";
             try
             {
                 InvokeRequestResponseService(Age, Name, Gender, IsIntention, Kurtosis, Skewness, secondDiffisNormalize, firstDiffisNormalize, percent_alpha, percent_beta, psd_alpha, psd_beta, arburg_1, arburg_2, arburg_3, arburg_4, arburg_5, arburg_6).Wait();

             }
             catch (Exception ex)
             {
              
             }
             OutputData outputdata= new OutputData()
             {
                // Istention = dataResultIStention,
                 Score = dataResultScore,
             };
             return outputdata;
         }
         private static Random random = new Random((int)DateTime.Now.Ticks);//thanks to McAden
         private string RandomString(int size)
         {
             StringBuilder builder = new StringBuilder();
             char ch;
             for (int i = 0; i < size; i++)
             {
                 ch = Convert.ToChar(Convert.ToInt32(Math.Floor(26 * random.NextDouble() + 65)));
                 builder.Append(ch);
             }

             return builder.ToString();
         }
         public PointData dataCloudTest(List<Double> data, CloudData cloud, int inten)
         {
             double[] test = data.ToArray();
             Descriptive desp = new Descriptive(test);
             desp.Analyze();
             LibHander output = new LibHander();
             FftEeg fe = new FftEeg();
             FftData fd = new FftData();
             fd = fe.Ffteeg(test, 512);
             Random rd = new Random();
             double[] arburg = output.arburg(test, 6);
             string Rand1 = RandomString(rd.Next(4, 10));
             int sum = 1;
             PointData entity = new PointData("dataprocess", "" + Rand1)
             {
                 percent_alpha = output.vget_percent_frequency(fd, "alpha"),
                 percent_beta = output.vget_percent_frequency(fd, "beta"),
                 psd_alpha = output.vget_energy(test, 512, "alpha"),
                 psd_beta = output.vget_energy(test, 512, "beta"),
                 Kurtosis = Tools.Kurtosis(test),
                 Skewness = Tools.Skewness(test),
                 arburg_1 = arburg[0],
                 arburg_2 = arburg[1],
                 arburg_3 = arburg[2],
                 arburg_4 = arburg[3],
                 arburg_5 = arburg[4],
                 arburg_6 = arburg[5],
                 secondDiffisNormalize = output.vget_mean_secondDiff(test, true),
                 firstDiffisNormalize = output.vget_mean_firstDiff(test, true),
                 Name = "Anhnph",
                 Age = 18,
                 IsIntention = inten,
                 Gender = "F",

             };
             return entity;
         }
         public PointData dataCloud(List<Double> data, CloudData cloud)
         {
             double[] test = data.ToArray();
             Descriptive desp = new Descriptive(test);
             desp.Analyze();
             LibHander output = new LibHander();
             FftEeg fe = new FftEeg();
             FftData fd = new FftData();
             fd = fe.Ffteeg(test, 512);
             Random rd = new Random();
             double[] arburg = output.arburg(test, 6);
             string Rand1 = RandomString(rd.Next(4,10));
             int sum = 1;
             PointData entity = new PointData("dataprocess", "" + Rand1)
             {
                 percent_alpha = output.vget_percent_frequency(fd, "alpha"),
                 percent_beta = output.vget_percent_frequency(fd, "beta"),
                 psd_alpha = output.vget_energy(test, 512, "alpha"),
                 psd_beta = output.vget_energy(test, 512, "beta"),
                 Kurtosis = Tools.Kurtosis(test),
                 Skewness = Tools.Skewness(test),
                 arburg_1 = arburg[0],
                 arburg_2 = arburg[1],
                 arburg_3 = arburg[2],
                 arburg_4 = arburg[3],
                 arburg_5 = arburg[4],
                 arburg_6 = arburg[5],
                 secondDiffisNormalize = output.vget_mean_secondDiff(test, true),
                 firstDiffisNormalize = output.vget_mean_firstDiff(test, true),
                 Name = cloud.Name,
                 Age = cloud.Age,
                 IsIntention = cloud.IsIntention,
                 Gender = cloud.Gender,
              
             };
             return entity;
         }
        public void InsertData(List<Double> data, CloudData cloud )
        {
            double[] test = data.ToArray();
            string accountName = "cloudthinkstorage";
            string accountKey = "u0gOpUjoxc9OWpjSBTSvm7tFHZPz8r8iFKK4uWxjtnC3Sh17oKytYMxR69lsfmGfkcmoQNmPPRWbD12l8QyNbg==";
            Descriptive desp = new Descriptive(test);
            desp.Analyze();
            LibHander output = new LibHander();
             
            try
            {
                StorageCredentials creds = new StorageCredentials(accountName, accountKey);
                CloudStorageAccount account = new CloudStorageAccount(creds, useHttps: true);
                CloudTableClient client = account.CreateCloudTableClient();
                CloudTable table = client.GetTableReference("cloudthink");
                table.CreateIfNotExists();
                Random rd = new Random();
                FftEeg fe = new FftEeg();
                FftData fd = new FftData();
                fd = fe.Ffteeg(test, 512);
                double [] arburg = output.arburg(test,6);
                PointData entity = new PointData("dataprocess", "" + rd.Next(1, 100000000))
                {
                    percent_alpha = output.vget_percent_frequency(fd, "alpha"),
                    percent_beta = output.vget_percent_frequency(fd, "beta"),
                    psd_alpha = output.vget_energy(test,512,"alpha"),
                    psd_beta = output.vget_energy(test, 512, "beta"),
                    Kurtosis = Tools.Kurtosis(test),
                    Skewness = Tools.Skewness(test),
                    arburg_1 = arburg[0],
                    arburg_2 = arburg[1],
                    arburg_3 = arburg[2],
                    arburg_4 = arburg[3],
                    arburg_5 = arburg[4],
                    arburg_6 = arburg[5],
                    secondDiffisNormalize = output.vget_mean_secondDiff(test, true),
                    firstDiffisNormalize = output.vget_mean_firstDiff(test, true),
                    Name = cloud.Name,
                    Age = cloud.Age,
                    IsIntention = cloud.IsIntention,
                    Gender = cloud.Gender
                };
                TableOperation inser = TableOperation.Insert(entity);
                table.Execute(inser);

            }
            catch (Exception ex)
            {

            }
             
        }
        
    }
}