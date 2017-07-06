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
        public int[] Genes { get; set; }
        private int[] planCount;
        private double[] usageCount;
        private readonly int _planNum;
        private readonly Tuple<long, double>[] _usage;
        public double Fitness { get; set; }
        public double TotalCost { get; set; }
        private Plan[] _plansInfo;
        private double _totalUsage;
        private double _usageAverage;
        public int Age { get; set; }
        private string id;

        public string Id
        {
            get { return id; }
            set { id = value; }
        }

        private Random rand;

        public DNA(int planNum, Plan[] plansInfo, double usageAverage, Tuple<long, double>[] usage)
        {
            rand = new Random(DateTime.Now.Millisecond);
            id = Guid.NewGuid().ToString();
            _planNum = planNum;
            Genes = new int[usage.Length];
            Age = 1;

            planCount = new int[planNum];
            usageCount = new double[planNum];
            _usage = usage;
            _plansInfo = plansInfo;
            _totalUsage = usageAverage*_usage.Length;
            _usageAverage = usageAverage;

            for (int i = 0; i < Genes.Length; i++)
            {
                if (i < Genes.Length/9)
                {
                    var r = PickOne(plansInfo);
                    Genes[i] = r;
                }
                else
                {
                    var r = rand.Next(planNum);// RandomGeneration.GetRandomNumber(planNum);
                    Genes[i] = r;

                }
                
            }
        }

        private int PickOne(Plan[] plansInfo)
        {
            var index = 0;
            var r = rand.NextDouble();// RandomGeneration.GetRandomDouble();

            while (r > 0)
            {
                r = r - plansInfo[index].Fitness;
                index++;
            }
            index--;
            return index;
        }

        public T[] Shuffle<T>(IEnumerable<T> items)
        {
            var result = items.ToArray();
            for (int i = items.Count(); i > 1; i--)
            {
                int j = rand.Next(i);// RandomGeneration.GetRandomNumber(i);
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
                usageCount[Genes[i]] += _usage[i].Item2;
            }

            TotalCost = 0;
            double totalPlancommitmentSum = 0;
            for (int i = 0; i < _planNum; i++)
            {
                var planInfo = PlanInformation.GetInfo(i);
                TotalCost += planCount[i] * planInfo.Cost;
                if (usageCount[i] > planInfo.Size * planCount[i])
                {
                    TotalCost += (usageCount[i] - planInfo.Size * planCount[i]) * planInfo.OverageCost;
                }
                totalPlancommitmentSum += planCount[i]* planInfo.Size;
            }
            

            Fitness = 1 * Math.Pow((1/TotalCost), 2) + 1/(totalPlancommitmentSum - _totalUsage) * 0;
            //Fitness = Math.Pow((1 / TotalCost), 3);
        }

        public void NormalizeFitness(double totalFitness, double maxCost, double minCost)
        {

            Fitness = (Fitness / totalFitness);

        }

        public DNA CrossOver(DNA partner)
        {
            DNA child = new DNA(_planNum, _plansInfo, _usageAverage, _usage);

            int midpoint1 = rand.Next(Genes.Length);// RandomGeneration.GetRandomNumber(Genes.Length); // Pick a midpoint
            int midpoint2 = rand.Next(Genes.Length); //RandomGeneration.GetRandomNumber(Genes.Length); // Pick a midpoint

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
                if(rand.NextDouble() < mutationRate)
                //if (RandomGeneration.GetRandomDouble() < mutationRate)
                {
                    var r1 = rand.Next(Genes.Length);// RandomGeneration.GetRandomNumber(Genes.Length);
                    var r2 = rand.Next(Genes.Length);//RandomGeneration.GetRandomNumber(Genes.Length);
                    var tmp = Genes[r2];
                    Genes[r2] = Genes[r1];
                    Genes[r1] = tmp;

                    //Genes[r1] = RandomGeneration.GetRandomNumber(_planNum);
                    //Genes[r1] = PickOne(_plansInfo);

                }
            }
        }

        public char GetRandomChar()
        {
            var chars = "abcdefghijklmnopqrstuvwxyz1234567890?,.ABCDEFGHIJKLMNOPQRSTUVWXYZ^& ".ToCharArray();
            int r = rand.Next(chars.Length);// RandomGeneration.GetRandomNumber(chars.Length);
            return chars[r];
        }
    }

}
