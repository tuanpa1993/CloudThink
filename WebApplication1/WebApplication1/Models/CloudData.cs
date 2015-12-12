using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;
namespace WebApplication1.Models
{
    public class CloudData 
    {
        public int IsIntention { get; set; }
        public int Age { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public string Gender { get; set; }
    }
}