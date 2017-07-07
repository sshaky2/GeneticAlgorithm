using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VZWCostOptimizationGA
{
    public class Population
    {
        private double _mutationRate;
        private DNA[] populationDNA;
        public int Generations { get; set; }
        public double TotalFitness { get; set; }
        private double BestFitness { get; set; }
        public DNA BestDNA { get; set; }
        public DNA WorstDNA { get; set; }
        private bool finished;
        private Tuple<long, double>[] _usage;
        private double perfectScore;
        private int _maxGeneration;
        private double _usageAverag;

        public double TargetAverage
        {
            get
            {
                return _targetAverage;
            }
        }

        public Plan[] PlansInfo { get; set; }

        public DNA[] DNAPopulation
        {
            get { return populationDNA; }
            set { populationDNA = value; }
        }

        public Tuple<long, double>[] UsageWithSim
        {
            get { return _usage; }
        }

        private Random rand;
        private double _targetAverage;
        

        public Population(double mutationRate, int popNumber, double targetAverage, Tuple<long, double>[] usage )
        {
            rand = new Random(DateTime.Now.Millisecond);
            _mutationRate = mutationRate;
            populationDNA = new DNA[popNumber];
            _usage = usage;
            _targetAverage = targetAverage;
            
            
            TotalFitness = 0;
            for (int i = 0; i < populationDNA.Length; i++)
            {
                populationDNA[i] = new DNA(_targetAverage, _usage);
                populationDNA[i].CalculateFitness();
               
                TotalFitness += populationDNA[i].Fitness;
            }
            BestFitness = 0;
            for (int i = 0; i < populationDNA.Length; i++)
            {
                populationDNA[i].NormalizeFitness(TotalFitness);
                if (populationDNA[i].Fitness > BestFitness)
                {
                    BestFitness = populationDNA[i].Fitness;
                    BestDNA = populationDNA[i];
                }
            }
            finished = false;
            Generations = 0;
            
        }


        public void GenerateBetter()
        {

            var p = rand.Next((populationDNA.Length - 1)/9);
            if (p%2 != 0) p++;
            var orderedPopulation = GetWorstToBestPopulation(populationDNA);
           
            for (int i = 0; i < p; i++)
            {
                var replaceIndex = -1;
                for (int x = 0; x < populationDNA.Length; x++)
                {
                    if (populationDNA[x].Id == orderedPopulation[i].Id)
                    {
                        replaceIndex = x;
                        break;
                    }
                }
                //int replaceIndex = Array.IndexOf(populationDNA, replace);
                var partnerA = RankBased(populationDNA);
                var partnerB = RankBased(populationDNA);
                DNA child = partnerA.CrossOver(partnerB);
                child.Mutate(_mutationRate);
                populationDNA[replaceIndex] = child;

            }

            TotalFitness = 0;
         
            for (int i = 0; i < populationDNA.Length; i++)
            {
                populationDNA[i].CalculateFitness();

                TotalFitness += populationDNA[i].Fitness;
                
            }
            
            BestFitness = 0;
            for (int i = 0; i < populationDNA.Length; i++)
            {
                populationDNA[i].NormalizeFitness(TotalFitness);
                if (populationDNA[i].Fitness > BestFitness)
                {
                    BestFitness = populationDNA[i].Fitness;
                    BestDNA = populationDNA[i];
                }
            }

            Generations++;
            
            
        }

        private DNA RankBased(DNA[] population)
        {
            
            var pop = population.OrderBy(x => x.Fitness).ToArray();
            double[] fitnes = new double[pop.Count()];
            double sum = (double)pop.Count() * (pop.Count() + 1)/2;
            for (int i = 0; i < population.Length; i++)
            {
                fitnes[i] = Convert.ToDouble((i + 1)/sum);
            }

            var index = 0;
            var r = rand.NextDouble();

            while (r > 0)
            {
                r = r - fitnes[index];
                index++;
            }
            index--;
            return pop[index];
        }
        
    

        public DNA[] GetWorstToBestPopulation(DNA[] population)
        {
            return population.OrderBy(x => x.Fitness).ToArray();

        }
        

        public bool Finished()
        {
            return finished;
        }

        int GetGeneration()
        {
            return Generations;
        }

   
    }
}
