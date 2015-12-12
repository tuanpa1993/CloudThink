using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using DiffLib;
namespace WebApplication1.Fus
{
    public class Genenate_Signal
    {
        public List<Double> Genenate(int dem)
        {
            double x=0;
            double y;
            List<Double> dl = new List<double>();
            int i = 0;
            const double pi = 3.141592;
            while (true)
            {
                
                y = Math.Sin(2 * pi * x) + Math.Sin(2 * pi * 2 * x);
                x = x + 0.05;
                dl.Add(y);
                i++;
                if (i == dem) break;
            }
           
            return dl;
        }
    }
}