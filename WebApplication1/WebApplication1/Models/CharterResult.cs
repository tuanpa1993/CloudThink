using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CharterResult
    {
        public DateTime TimeChart { get; set; }
        public int Intention { get; set; }
        public int NonIntention { get; set; }
    }
}