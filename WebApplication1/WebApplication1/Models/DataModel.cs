using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebApplication1.Models
{
    public class DataModel : TableEntity
    {
        public DataModel(string cate, string sku) : base(cate, sku)
        {

        }
        public DataModel() { }
        public int Age { get; set; }
        public string dataraw { get; set; }
        public string Name { get; set; }
        public string Gender { get; set; }
        public int IsIntention { get; set; }
        public double Entropy { get; set; }
        public double StandardDeviation { get; set; }
        public double Mean { get; set; }
        public double Kurtosis { get; set; }
        public double Skewness { get; set; }
        public double Max { get; set; }
        public double StdDev { get; set; }
        public double secondDiff { get; set; }
        public double firstDiff { get; set; }
        public double secondDiffisNormalize { get; set; }
        public double firstDiffisNormalize { get; set; }


    }
}