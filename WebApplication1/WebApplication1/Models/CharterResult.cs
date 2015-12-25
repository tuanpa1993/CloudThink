using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace WebApplication1.Models
{
    public class CharterResult
    {
        public int Hour { get; set; }
        public int Intention { get; set; }
        public int NonIntention { get; set; }
        public DateTime LastTime { get; set; }
    }
}