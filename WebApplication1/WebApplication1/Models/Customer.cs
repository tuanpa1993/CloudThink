using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace WebApplication1.Models
{
    public class Customer
    {
        public int Age { get; set; }
        public string Name { get; set; }
        public string Data { get; set; }
        public string Gender { get; set; }

       
    }
}