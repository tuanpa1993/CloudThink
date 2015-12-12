using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using Microsoft.WindowsAzure.Storage.Table;
using StatDescriptive;
using NUnit.Framework;
using Accord.Statistics;
using CenterSpace.NMath.Core;
using WebApplication1.Fus;
using MathWorks.MATLAB.NET;
using MathWorks.MATLAB.NET.ComponentData;
using WebApplication1.Models;
using MathNet.Numerics;
using MathNet.Numerics.Transformations;
using AForge.Math;

namespace WebApplication1.Fus
{
    public class LibHander
    {
        public List<Double> Diff(double[] ab){
            List<Double> dl = new List<double>();
            for (int i = 1; i < ab.Length; i++)
            {
                dl.Add(ab[i] - ab[i - 1]);
            }
                return dl;
        }
        public List<Double> Diff(double[] ab, int n)
        {
            List<Double> dl = new List<double>();
            int dem = 1;
            while (true)
            {
                dl.Clear();
                for (int i = 1; i < ab.Length; i++)
                {
                    dl.Add(ab[i] - ab[i - 1]);
                }
                ab = dl.ToArray();
                if (dem == n) break;
                dem = dem + 1;
            }

            return dl;
        }
        public double vget_mean_firstDiff(double[] ab,Boolean bl)
        {
            List<Double> dl = new List<double>();
            for (int i = ab.Length-1; i >= 0; i--)
            {
                dl.Add(ab[i]);
            }
           
            double[] data = dl.ToArray();
            if (bl == true)
            {
                dl.Clear();
                for (int i = 0; i < data.Length; i++)
                {
                    dl.Add(data[i] / data.Max());
                }
                data = dl.ToArray();
            }
            double[] diff_value = Diff(data).ToArray();
            List<Double> dls = new List<double>();
            for (int i = 0; i < diff_value.Length ; i++)
            {
                dls.Add(Math.Abs(diff_value[i]));
            }

            
            double vget_mean_firstDiff = dls.ToArray().Sum() / (dls.ToArray().Length-1);
            return vget_mean_firstDiff;
        }
        public double vget_percent_frequency(FftData data, string wave)
        {
            GetWave gw = new GetWave();
            double[] freq_amplitude = data.Fft_Data;
            double[] datawave = gw.getWave(wave, data);
            double wave_amplitude = datawave.Sum();
            double total_amplitude = freq_amplitude.Sum();
            double percent_value = wave_amplitude / total_amplitude;
            return percent_value;
        }
        public double nextpow2(int length)
        {
            int n = 1;
            while (n < length)
            {
                n = n * 2;
            }
            return n;
        }
        public double[] arburg(double[] inputseries, int degree)
        {
            double[] h = null;
            double[] g = null;      /* Used by mempar()                              */
            double[] per = null;
            double[] pef = null;      /* Used by mempar()                              */
            double[] conf = null;
            int length = inputseries.Length;

            h = new double[degree + 1];

            g = new double[degree + 2];

            per = new double[length + 1];

            pef = new double[length + 1];


            conf = new double[degree];


            ARMaxEntropy(inputseries, length, degree, conf, per, pef, h, g);

            return conf;
        }
        public void ARMaxEntropy(double[] inputseries, int length, int degree,
                double[] coef,
                double[] per, double[] pef, double[] h, double[] g)
        {
            double t1, t2;
            int n;

            for (n = 1; n <= degree; n++)
            {
                double sn = 0.0;
                double sd = 0.0;
                int j;
                int jj = length - n;

                for (j = 0; j < jj; j++)
                {
                    t1 = inputseries[j + n] + pef[j];
                    t2 = inputseries[j] + per[j];
                    sn -= 2.0 * t1 * t2;
                    sd += (t1 * t1) + (t2 * t2);
                }

                t1 = g[n] = sn / sd;
                if (n != 1)
                {
                    for (j = 1; j < n; j++)
                        h[j] = g[j] + t1 * g[n - j];
                    for (j = 1; j < n; j++)
                        g[j] = h[j];
                    jj--;
                }

                for (j = 0; j < jj; j++)
                {
                    per[j] += t1 * pef[j] + t1 * inputseries[j + n];
                    pef[j] = pef[j + 1] + t1 * per[j + 1] + t1 * inputseries[j + 1];
                }


            }

            for (n = 0; n < degree; n++)
                coef[n] = g[n + 1];
        }
        public double vget_energy(double[] data , int fs,string wavetype){
            int nlength = data.Length;
            double pow = nextpow2(nlength);
            double nfft;
            int checkdev = nlength % 2;
            if (checkdev == 1)
            {
                nfft = nlength + 1;
            }
            else nfft = nlength;
            double lowB, hightB;
            GetWaveRange gwr = new GetWaveRange();
            GetWaveData gwd = new GetWaveData();
            List<Double> dl = new List<double>();
            double logLength = Math.Ceiling(Math.Log((double)data.Length, 2.0));
            int paddedLength = (int)Math.Pow(2.0, Math.Min(Math.Max(1.0, logLength), 14.0));
            AForge.Math.Complex[] complex = new AForge.Math.Complex[paddedLength];
            for (int i = 1; i < data.Length; i++)
            {
                complex[i] = new AForge.Math.Complex(data[i], 0);
            }
            FourierTransform.FFT(complex, FourierTransform.Direction.Forward);
            for (int i = 0; i < nlength-1; i++)
            {
                string split = complex.GetValue(i + 1).ToString();
                split = split.Replace(" ", "");
                split = split.Replace("(", "");
                split = split.Replace(")", "");
                string[] split_datas = split.Split(',');
                data[i] = double.Parse(split_datas[0]);

            }
            int range = (int)nfft / 2 + 1;
            double[] freq = Generate.LinearSpaced(range, 0.0, 1.0);
            for (int i = 0; i < freq.Length; i++)
            {
                dl.Add(fs / 2 * freq[i]);
            }
            freq = dl.ToArray();
            dl.Clear();
            //thieu ham

            //
            for (int i = 0; i < data.Length; i++)
            {
                dl.Add(data[i] / nlength);
            }
           gwd= gwr.getWaveRange(wavetype);
           lowB = gwd.LowBound;
           hightB = gwd.HighBound;
           double[] psd = dl.ToArray();
            dl.Clear();
            for (int i = 0; i < freq.Length; i++)
            {
                int datr= (int) freq[i];
                if (lowB <= freq[i] && freq[i] < hightB)
                    dl.Add(psd[datr]);
            }
            double point = dl.Sum();
            EnergyData enr = new EnergyData();
            return point;
        } 
        public double vget_mean_secondDiff(double[] ab,Boolean bl)
        {
            List<Double> dl = new List<double>();
            for (int i = ab.Length - 1; i >= 0; i--)
            {
                dl.Add(ab[i]);
            }
            double[] data = dl.ToArray();
            if (bl == true)
            {
                dl.Clear();
                for (int i = 0; i < data.Length; i++)
                {
                    dl.Add(data[i] / data.Max());
                }
                data = dl.ToArray();
            }
            double[] diff_value = Diff(data,2).ToArray();
            List<Double> dls = new List<double>();
            for (int i = 0; i < diff_value.Length; i++)
            {
                dls.Add(Math.Abs(diff_value[i]));
            }


            double vget_mean_secondDiff = dls.ToArray().Sum() / (dls.ToArray().Length - 2);
            return vget_mean_secondDiff;
        }
        public double Entropy(double[] ab)
        {
            double entropy=0;

            return entropy;
                 
        }
       
    }
}