using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebApplication1.Models
{
    public class CharterData : TableEntity
    {
        public CharterData(string cate, string sku)
            : base(cate, sku)
        {

        }
        public CharterData() { }
        public int Duration { get; set; }
        public string datetime { get; set; }
        public string Name { get; set; }
        public int Istention { get; set; }
    }
}