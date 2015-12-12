using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage.Table;

namespace WebApplication1.Models
{
    public class Profile : TableEntity
    {
        public Profile(string cate, string sku) : base(cate, sku)
        {

        }
        public Profile() { }
        public int Age { get; set; }
        public string UserName { get; set; }
        public string PassWord { get; set; }
        public string Email { get; set; }
        public string Gender { get; set; }
    }
}