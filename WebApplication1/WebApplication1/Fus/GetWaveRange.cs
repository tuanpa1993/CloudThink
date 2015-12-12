using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;

namespace WebApplication1.Fus
{
    public class GetWaveRange
    {
        
        public GetWaveData getWaveRange(string wave_type)
        {
            double lowB;
            double hightB;
            switch (wave_type)
            {
                case "delta": {
                    lowB = 0.5;
                    hightB = 3.99;
                    break;
                }
                case "theta":
                    {
                        lowB = 4;
                        hightB = 7.99;
                        break;
                    }
                case "alpha":
                    {
                        lowB = 8;
                        hightB = 13.99;
                        break;
                    }
                case "beta":
                    {
                        lowB = 14;
                        hightB = 30;
                        break;
                    }
                case "gamma":
                    {
                        lowB = 30;
                        hightB = 128;
                        break;
                    }
                default : {
                    lowB = 0;
                    hightB = 0;
                    break;
                }

            }
            GetWaveData getWR = new GetWaveData()
            {
                LowBound = lowB,
                HighBound = hightB
            };
            return getWR;

        }
    }
}