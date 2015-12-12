using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebApplication1.Models
{
    public class PointData : TableEntity
    {
        public PointData(string cate, string sku) 
        {
            this.PartitionKey = cate;
            this.RowKey = sku;
        }
        public PointData() {}
        public int Age { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int IsIntention { get; set; }
        public double Kurtosis { get; set; }
        public double Skewness { get; set; }
        public double secondDiffisNormalize { get; set; }
        public double firstDiffisNormalize { get; set; }
        public double percent_alpha { get; set; }
        public double percent_beta { get; set; }
        public double psd_alpha { get; set; }
        public double psd_beta { get; set; }
        public double arburg_1 { get; set; }
        public double arburg_2 { get; set; }
        public double arburg_3 { get; set; }
        public double arburg_4 { get; set; }
        public double arburg_5 { get; set; }
        public double arburg_6 { get; set; }
    }
}