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
        private readonly Tuple<long, double>[] _usage;
        public double Fitness { get; set; }
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
        private double _targetAverage;
        private int _subsetMemberCount;
        public double GeneAverage { get; set; }

        public DNA(double targetAverage, Tuple<long, double>[] usage)
        {
            rand = new Random(DateTime.Now.Millisecond);
            id = Guid.NewGuid().ToString();
            Genes = new int[usage.Length];
            Age = 1;
            
            _usage = usage;
            _targetAverage = targetAverage;

            for (int i = 0; i < Genes.Length; i++)
            {
                Genes[i] = rand.Next(2);
               
            }
        }

   
        public void CalculateFitness()
        {
            _subsetMemberCount = 0;
            double total = 0;
            for (int i = 0; i < Genes.Length; i++)
            {
                if (Genes[i] == 1)
                {
                    _subsetMemberCount++;
                    total += _usage[i].Item2;
                }
               
            }
            GeneAverage = total/_subsetMemberCount;
            Fitness = Math.Pow(1 / Math.Abs(_targetAverage - GeneAverage), 2);
            
        }

        public void NormalizeFitness(double totalFitness)
        {

            Fitness = (Fitness / totalFitness);

        }

        public DNA CrossOver(DNA partner)
        {
            DNA child = new DNA(_targetAverage, _usage);

            int midpoint1 = rand.Next(Genes.Length);
            int midpoint2 = rand.Next(Genes.Length); 

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
