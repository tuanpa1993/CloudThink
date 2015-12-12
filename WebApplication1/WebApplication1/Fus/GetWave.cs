using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using WebApplication1.Fus;
namespace WebApplication1.Fus
{
    public class GetWave
    {
        public double[] getWave(string wave_type,FftData data){
            double[] fft_data = data.Fft_Data;
            double[] fft_freq = data.Fft_Freq;
            List<Double> dl = new List<double>();
            GetWaveData gw = new GetWaveData();
            GetWaveRange gwr = new GetWaveRange();
            gw = gwr.getWaveRange(wave_type);
            double lowb = gw.LowBound;
            double hightb = gw.HighBound;
            for (int i = 0; i<fft_freq.Length; i++)
            {
                if (fft_freq[i] >= lowb && fft_freq[i] <= hightb)
                {
                    dl.Add(fft_freq[i]);
                }
            }

            fft_freq = dl.ToArray();
            dl.Clear();
            for (int i = 0; i<fft_freq.Length; i++){
                int fft_fil = (int)fft_freq[i];
                dl.Add(fft_data[fft_fil]);
            }
            fft_data = dl.ToArray();
            return fft_data;

        }
    }
}