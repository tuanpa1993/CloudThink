
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using WebApplication1.Models;
using NUnit.Framework;
using MathNet.Numerics;
using MathNet.Numerics.Transformations;
using Accord.Statistics;
using CenterSpace.NMath.Core;
using AForge.Math;

namespace WebApplication1.Fus
{
    public class FftEeg
    {
        public double nextpow2(int length)
        {
            int n = 1;
            while (n < length)
            {
                n = n * 2;
            }
            return n;
        }
        public FftData Ffteeg(double[] data,int fs)
        {
            List<Double> dl = new List<double>();       
            for (int i = data.Length - 1; i >= 0; i--)
            {
                dl.Add(data[i]);
            }
            double[] data_value = dl.ToArray();
            dl.Clear();
            int nlength = data_value.Length;
            int checkdev = nlength % 2;
            double nfft;
            if (checkdev == 1)
            {
                nfft = nlength + 1;
            }
            else nfft = nlength;
            double logLength = Math.Ceiling(Math.Log((double)data_value.Length, 2.0));
            int paddedLength = (int)Math.Pow(2.0, Math.Min(Math.Max(1.0, logLength), 14.0));

            AForge.Math.Complex[] complex = new AForge.Math.Complex[paddedLength];
            for (int i = 1; i < data_value.Length; i++)
            {
                complex[i] = new AForge.Math.Complex(data_value[i], 0);
            }
            FourierTransform.FFT(complex , FourierTransform.Direction.Forward);
           
      
                for (int i = 0; i < nlength-1; i++)
                {
                    string split = complex.GetValue(i+1).ToString();
                    split = split.Replace(" ", "");
                    split = split.Replace("(", "");
                    split = split.Replace(")", "");
                    string[] split_datas = split.Split(',');
                   data_value[i] = double.Parse(split_datas[0]);
               
                }
            double fmax = data_value.Max();
            for (int i = 0; i < data_value.Length; i++)
            {
                dl.Add(fmax / data_value[i]);

            }
            data_value = dl.ToArray();
            dl.Clear();
            int a = (int)(nfft / 2) + 1;
            for (int i = 0; i <data_value.Length; i++)
            {
            //    if (i > a) break;
        //        else
                dl.Add(data_value[i]);
            }
            double[] fft_value =  dl.ToArray();
            dl.Clear();
            for (int i = 0; i < fft_value.Length; i++)
            {
                dl.Add(2*Math.Abs(fft_value[i]));
            }
          //  Generate.LinearSpaced(11, 0.0, 1.0);
            fft_value = dl.ToArray();
            dl.Clear();
            int range = (int)nfft / 2 +1;
            double[] freq = Generate.LinearSpaced(range , 0.0 , 1.0 ); 
            for (int i = 0; i < freq.Length; i++)
            {
                 dl.Add(fs/2*freq[i]);
            }
            freq = dl.ToArray();
            dl.Clear();
            for (int i = 0; i < freq.Length; i++)
            {
                if (freq[i] >= 0.5) dl.Add(freq[i]);
            }
            double [] data_freq = dl.ToArray();
            dl.Clear();
            for (int i = 0; i < data_freq.Length; i++)
            {   
                int data_fft =(int) data_freq[i];
                dl.Add(fft_value[data_fft]);
            }
            fft_value = dl.ToArray();
            dl.Clear();
            //for (int i = 0; i < data_freq.Length; i++)
            //{
            //    int data_fft = (int)data_freq[i];
            //    dl.Add(freq[data_fft]);
            //}
            //freq = dl.ToArray();
          //  double freq = ;
            FftData data_eeg = new FftData(){
                Fft_Data = fft_value,
                Fft_Freq = data_freq
            };
            return data_eeg;
        }
    }
}