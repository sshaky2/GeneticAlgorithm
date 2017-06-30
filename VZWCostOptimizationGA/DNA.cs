using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;

namespace VZWCostOptimizationGA
{
    public class DNA
    {
        //public int[] Genes { get; set; }
        public int[] Genes { get; set; }
        private int[] planCount;
        private double[] usageCount;
        private readonly int _planNum;
        private readonly double[] _usage;
        public double Fitness { get; set; }
        public double TotalCost { get; set; }
        private Plan[] _plansInfo;



        public DNA(int geneSize, int planNum, Plan[] plansInfo, double[] usage)
        {
            _planNum = planNum;
            Genes = new int[geneSize];

            planCount = new int[planNum];
            usageCount = new double[planNum];
            _usage = usage;
            _plansInfo = plansInfo;

            for (int i = 0; i < Genes.Length; i++)
            {
                var r = PickOne(plansInfo);
                //var r = RandomGeneration.GetRandomNumber(planNum);
                //var list = Shuffle(new List<int> {4,3,2,0});
                //Genes[i] = list[0];
                Genes[i] = r;
            }
           
            //for (int i = 0; i < target.Length; i++)
                //{
                //    Genes[i] = GetRandomChar();  // Pick from range of chars
                //    //Console.WriteLine(Genes[i]);
                //}
        }

        private int PickOne(Plan[] plansInfo)
        {
            var index = 0;
            var r = RandomGeneration.GetRandomDouble();

            while (r > 0)
            {
                r = r - plansInfo[index].Fitness;
                index++;
            }
            index--;
            return index;
        }

        public static T[] Shuffle<T>(IEnumerable<T> items)
        {
            var result = items.ToArray();
            for (int i = items.Count(); i > 1; i--)
            {
                int j = RandomGeneration.GetRandomNumber(i);
                var t = result[j];
                result[j] = result[i - 1];
                result[i - 1] = t;
            }

            return result;
        }

        public void CalculateFitness()
        {
            for (int i = 0; i < planCount.Length; i++)
            {
                planCount[i] = 0;
            }
            for (int i = 0; i < usageCount.Length; i++)
            {
                usageCount[i] = 0;
            }
            for (int i = 0; i < Genes.Length; i++)
            {
                planCount[Genes[i]]++;
                usageCount[Genes[i]] += _usage[i];
            }

            TotalCost = 0;
            for (int i = 0; i < _planNum; i++)
            {
                var planInfo = PlanInformation.GetInfo(i);
                TotalCost += planCount[i] * planInfo.Cost;
                if (usageCount[i] > planInfo.Size * planCount[i])
                {
                    TotalCost += (usageCount[i] - planInfo.Size * planCount[i]) * planInfo.OverageCost;
                }
            }

            

            Fitness = Math.Pow(1/TotalCost, 3);
            //Fitness = TotalCost;
        }

        //public void CalculateFitness()
        //{
        //    //CalculateTotalCost();

        //    Fitness = (1 / TotalCost + 1);
        //    //double min = 1/maxCost;
        //    //double max = 1/10000.00;
        //    //double diff = Math.Abs(max - min);
        //    //Fitness = (Fitness - min)/diff;
        //    //Fitness = Math.Pow(Fitness, 3);



        //    //int score = 0;
        //    //for (int i = 0; i < Genes.Length; i++)
        //    //{
        //    //    if (Genes[i] == target[i])
        //    //    {
        //    //        score++;
        //    //    }
        //    //}
        //    //Fitness = (double)score / (double)target.Length;
        //    //Fitness = Math.Pow(Fitness, 2) + 0.01;
        //}

        public void NormalizeFitness(double totalFitness, double maxCost, double minCost)
        {

            ////Fitness = (1 / (TotalCost + 1));
            //double min = 1 / maxCost;
            //double max = 1 / minCost;
            //double diff = Math.Abs(max - min);
            //Fitness = (Fitness - min) / diff;
            //Fitness = (Fitness - min) / totalFitness;
            //Fitness = Math.Pow(Fitness, 3);

            Fitness = (Fitness / totalFitness);
            //Fitness = Math.Pow(Fitness, 3);

        }

        public DNA CrossOver(DNA partner)
        {
            DNA child = new DNA(Genes.Length, _planNum, _plansInfo, _usage);

            int midpoint1 = RandomGeneration.GetRandomNumber(Genes.Length); // Pick a midpoint
            int midpoint2 = RandomGeneration.GetRandomNumber(Genes.Length); // Pick a midpoint

            int start = Math.Min(midpoint1, midpoint2);
            int stop = Math.Max(midpoint1, midpoint2);
            for (int i = 0; i < Genes.Length; i++)
            {
                if (i >= start && i < stop)
                {
                    child.Genes[i] = partner.Genes[i];
                }
                else
                {
                    child.Genes[i] = Genes[i];
                }
            }

            // Half from one, half from the other
            //for (int i = 0; i < Genes.Length; i++)
            //{
            //    if (i > midpoint) child.Genes[i] = Genes[i];
            //    else child.Genes[i] = partner.Genes[i];
            //}
            return child;
        }

        public void Mutate(double mutationRate)
        {
            for (int i = 0; i < Genes.Length; i++)
            {
                if (RandomGeneration.GetRandomDouble() < mutationRate)
                {
                    //Genes[i] = PickOne(_plansInfo);
                    //Genes[i] = RandomGeneration.GetRandomNumber(_planNum);
                    //Genes[i] = GetRandomChar();

                    var r1 = RandomGeneration.GetRandomNumber(Genes.Length);
                    var r2 = RandomGeneration.GetRandomNumber(Genes.Length);
                    var tmp = Genes[r2];
                    Genes[r2] = Genes[r1];
                    Genes[r1] = tmp;

                }
            }
        }

        public char GetRandomChar()
        {
            var chars = "abcdefghijklmnopqrstuvwxyz1234567890?,.ABCDEFGHIJKLMNOPQRSTUVWXYZ^& ".ToCharArray();
            int rand = RandomGeneration.GetRandomNumber(chars.Length);
            return chars[rand];
        }
    }

}
