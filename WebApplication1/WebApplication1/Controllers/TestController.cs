using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web.Http;
using WebApplication1.Models;
using WebApplication1.Fus;
using StatDescriptive;
using NUnit.Framework;
using Accord.Statistics;
using CenterSpace.NMath.Core;

namespace WebApplication1.Controllers
{
    public class TestController : ApiController
    {
        [HttpGet]
        public IHttpActionResult Get([FromUri]CloudData cloud)
        {
            double[] data ={683,630,770,610,726,423,647,645,416,615,529,526,603,887,648,695,659,361,567,492,584,291,755,531,625,611,386,489,441,479,354,452,357,485,418,602,499,416,364,364,439,365,593,421,613,369,410,405,412,402,383,418,415,696,359,442,403,423,386,402,386,397,388,344,579,357,462,397,363,368,456,359,341,375,391,629,366,371,402,364,397,399,376,372,477,405,649,374,519,421,399,448,469,436,518,669,461,676,413,367,441,454,384,328,476,557,670,429,569,457,668,477,439,518,639,744,634,769,593,814,535,628,555,502,411,598,844,354,737,577,710,544,612,630,467,655,582,723,546,772,704,575,643,287,602,609,723,589,757,632,679,632,547,626,594,663,579,870,459,769,574,479,574,464,493,353,394,401,684,417,476,410,335,425,349,406,366,488,404,527,526,587,393,477,425,395,431,392,702,365,619,370,496,354,368,399,387,384,338,595,399,452,380,431,371,342,416,408,323,368,663,376,489,349,434,412,351,376,401,595,386,405,409,405,382,381,381,379,412,414,603,428,589,483,484,359,457,443,455,408,426,799,335.0
};
            LibHander lbh = new LibHander();
            FftEeg fe = new FftEeg();
            FftData fd = new FftData();
            fd = fe.Ffteeg(data, 256);
            double percent_alpha = lbh.vget_percent_frequency(fd, "alpha");
            double percent_beta = lbh.vget_percent_frequency(fd, "beta");
            double psd_alpha = lbh.vget_energy(data,256,"alpha");
            double psd_beta = lbh.vget_energy(data, 256, "beta");
            double mean_first = lbh.vget_mean_firstDiff(data, true);
            double mean_second = lbh.vget_mean_secondDiff(data, true);
            double Kurtosis = Tools.Kurtosis(data) ;
            double Skewness = Tools.Skewness(data);
            return Ok("percent_alpha: " + percent_alpha + "  " + "percent_beta: " + percent_beta + "   psd_alpha: " + psd_alpha + "   psd_beta: " + psd_beta
                + "   mean_first: " + mean_first + "   mean_second: " + mean_second + "   Kurtosis: " + Kurtosis + "   Skewness: " + Skewness);
        }
    }
}
