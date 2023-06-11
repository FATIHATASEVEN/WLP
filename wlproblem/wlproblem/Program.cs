using System;
using System.IO;
using System.Linq;
using System.Collections.Generic;
using System.Threading;


namespace WarehouseLocationProblemWLP
{
    class WLP
    {
        public static void Main(string[] args)
        {
            string dyolu = "200veri";
            List<double> depoKapasiteListesi = new List<double>();
            List<double> depoMaliyetListe= new List<double>();
            int depoSayisi;
            int müşteriSayisi;
            List<int> musteriTalepList = new List<int>();
            using (StreamReader stread = new StreamReader(dyolu))
            {
                string[] veri = File.ReadAllLines(dyolu);
                string line = stread.ReadLine();
                string[] firstLine = line.Split(' ');
                depoSayisi = int.Parse(firstLine[0].Trim());
                müşteriSayisi = int.Parse(firstLine[1].Trim());
                for (int i = 0; i < depoSayisi; i++)
                {
                    line = stread.ReadLine();
                    string[] lines = line.Split(' ');
                    depoKapasiteListesi.Add(double.Parse(lines[0].Trim()));
                    depoMaliyetListe.Add(double.Parse(lines[1].Trim()));
                }
            }
            double[,] depoicinmusterimaaliyet = new double[müşteriSayisi, depoSayisi];
            using (StreamReader str = new StreamReader(dyolu))
            {
                string[] datas = File.ReadAllLines(dyolu);
                int j = 0;
                int l = 0;
                for (int i = depoSayisi + 1; i < (müşteriSayisi * 2) + 1 + depoSayisi; i++)
                {
                    if (i % 2 != 0)
                    {
                        musteriTalepList.Add(int.Parse(datas[i]));
                    }
                    else
                    {
                        string[] data = datas[i].Split(' ');
                        for (int k = 0; k < depoSayisi; k++)
                        {
                            depoicinmusterimaaliyet[l, k] = double.Parse(data[k]);
                        }
                        l++;
                    }
                    j++;
                }
            }
            int[] depoKapasiteler = depoKapasiteListesi.Select(x => (int)x).ToArray();
            double[] depomaliyetleri = depoMaliyetListe.Select(x => (double)x).ToArray();
            int[] musteriTalepleri = musteriTalepList.Select(x => (int)x).ToArray();
            int[] atanandepo = new int[müşteriSayisi];
            double toplamtutar = SolveWLP(depoSayisi, müşteriSayisi, depoKapasiteler, depomaliyetleri, musteriTalepleri, depoicinmusterimaaliyet, atanandepo);
            Console.WriteLine("EKMEL MAALİYET =  " + toplamtutar);
            Console.WriteLine(string.Join(" ", atanandepo.Select((w, i) => $"{w}")));
        }
        static double SolveWLP(int depoadeti, int musterisayisi, int[] depokapasite, double[] depomaaliyet, int[]musteritalepler, double[,] depomustmaaliyet, int[] atanandepo)
        {
            double tutartop = 0;
            bool[] kullanılandepo = new bool[depoadeti];
            for (int c = 0; c < musterisayisi; c++)
            {
                int optimaldepo = -1;
                double optmaaliyet = double.MaxValue;
                for (int w = 0; w < depoadeti; w++)
                {
                    if (depokapasite[w] >= musteritalepler[c] && depomustmaaliyet[c, w] <  optmaaliyet)
                    {
                        optimaldepo = w;
                        optmaaliyet = depomustmaaliyet[c, w];
                    }
                }
                if (optimaldepo != -1)
                {
                    atanandepo[c] = optimaldepo;
                    depokapasite[optimaldepo] -= musteritalepler[c];
                    tutartop += optmaaliyet;
                    if (!kullanılandepo[optimaldepo])
                    {
                        tutartop += depomaaliyet[optimaldepo];
                        kullanılandepo[optimaldepo] = true;
                    }
                }
            }
            return tutartop;
        }
    }
}