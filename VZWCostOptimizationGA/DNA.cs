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
        public DNA(int geneSize, int planNum, double[] usage)
        {
            _planNum = planNum;
            Genes = new int[geneSize];

            planCount = new int[planNum];
            usageCount = new double[planNum];
            _usage = usage;

            for (int i = 0; i < Genes.Length; i++)
            {
                var r = RandomGeneration.GetRandomNumber(planNum);
                Genes[i] = r;
            }
           
            //for (int i = 0; i < target.Length; i++)
                //{
                //    Genes[i] = GetRandomChar();  // Pick from range of chars
                //    //Console.WriteLine(Genes[i]);
                //}
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

            Fitness = (1 / (TotalCost + 1));
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

            Fitness = (1 / (TotalCost + 1));
            double min = 1 / maxCost;
            double max = 1 / 10000.00;
            double diff = Math.Abs(max - min);
            Fitness = (Fitness - min) / diff;
            Fitness = Math.Pow(Fitness, 3);
            //Fitness = Math.Pow((Fitness/totalFitness),2);
        }

        public DNA CrossOver(DNA partner)
        {
            DNA child = new DNA(Genes.Length, _planNum, _usage);

            int midpoint = RandomGeneration.GetRandomNumber(Genes.Length); // Pick a midpoint

            // Half from one, half from the other
            for (int i = 0; i < Genes.Length; i++)
            {
                if (i > midpoint) child.Genes[i] = Genes[i];
                else child.Genes[i] = partner.Genes[i];
            }
            return child;
        }

        public void Mutate(double mutationRate)
        {
            for (int i = 0; i < Genes.Length; i++)
            {
                var abc = RandomGeneration.GetRandomDouble();
                if (abc < mutationRate)
                {
                    Genes[i] = RandomGeneration.GetRandomNumber(_planNum);
                    //Genes[i] = GetRandomChar();
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
