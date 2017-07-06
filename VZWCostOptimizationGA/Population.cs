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
        public double MinCost { get; set; }
        public double MaxCost { get; set; }
        public double TotalFitness { get; set; }
        public double BestCost { get; set; }
        public double WorstCost { get; set; }
        public DNA BestDNA { get; set; }
        public DNA WorstDNA { get; set; }
        private bool finished;
        private Tuple<long, double>[] _usage;
        private double perfectScore;
        private int _maxGeneration;
        private double _usageAverag;

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

        public Population(double mutationRate, int popNumber, int planNum, int maxGeneration, double usageAverage, Tuple<long, double>[] usage )
        {
            rand = new Random(DateTime.Now.Millisecond);
            _mutationRate = mutationRate;
            populationDNA = new DNA[popNumber];
            _usage = usage;
            _usageAverag = usageAverage;
            PlansInfo = new Plan[planNum];
            double totalPlanFitness = 0;
            for (int i = 0; i < planNum; i++)
            {
                PlansInfo[i] = PlanInformation.GetInfo(i);
                PlansInfo[i].Fitness = 1/Math.Pow(Math.Abs(PlansInfo[i].Size - usageAverage), 2);
                totalPlanFitness += PlansInfo[i].Fitness;
            }
            NormalizePlanFitness(totalPlanFitness, planNum);

            MaxCost = 0;
            MinCost = double.MaxValue;
            TotalFitness = 0;
            for (int i = 0; i < populationDNA.Length; i++)
            {
                populationDNA[i] = new DNA(planNum, PlansInfo, _usageAverag, _usage);
                populationDNA[i].CalculateFitness();
                if (populationDNA[i].TotalCost > MaxCost)
                {
                    MaxCost = populationDNA[i].TotalCost;
                   
                }
                if (populationDNA[i].TotalCost < MinCost)
                {
                    MinCost = populationDNA[i].TotalCost;
                }

                TotalFitness += populationDNA[i].Fitness;
            }
            BestCost = Double.MaxValue;
            for (int i = 0; i < populationDNA.Length; i++)
            {
                populationDNA[i].NormalizeFitness(TotalFitness, MaxCost, MinCost);
                if (populationDNA[i].TotalCost < BestCost)
                {
                    BestCost = populationDNA[i].TotalCost;
                    BestDNA = populationDNA[i];
                }
            }
            finished = false;
            Generations = 0;
            perfectScore = 1;
            _maxGeneration = maxGeneration;
            
        }

        private void NormalizePlanFitness(double totalPlanFitness, int planNum)
        {
            double totalNormalizedFItness = 0;
            for (int i = 0; i < planNum; i++)
            {
                PlansInfo[i].Fitness = PlansInfo[i].Fitness/totalPlanFitness;
                totalNormalizedFItness += PlansInfo[i].Fitness;
            }
        }
        
        public void Generate()
        {

            DNA[] newPopulation = new DNA[populationDNA.Length];
            for (int i = 0; i < populationDNA.Length; i++)
            {
                var partnerA = RankBased(populationDNA);
                var partnerB = RankBased(populationDNA);
                DNA child = partnerA.CrossOver(partnerB);
                child.Mutate(_mutationRate);
                newPopulation[i] = child;
            }
            TotalFitness = 0;
            MaxCost = 0;
            MinCost = double.MaxValue;
            for (int i = 0; i < newPopulation.Length; i++)
            {
                populationDNA[i] = newPopulation[i];
                populationDNA[i].CalculateFitness();
                if (populationDNA[i].TotalCost > MaxCost)
                {
                    MaxCost = populationDNA[i].TotalCost;
                }
                if (populationDNA[i].TotalCost < MinCost)
                {
                    MinCost = populationDNA[i].TotalCost;
                }

                TotalFitness += populationDNA[i].Fitness;
               

            }
            BestCost = double.MaxValue;
            WorstCost = 0;
            for (int i = 0; i < newPopulation.Length; i++)
            {
                populationDNA[i].NormalizeFitness(TotalFitness, MaxCost, MinCost);
                if (populationDNA[i].TotalCost < BestCost)
                {
                    BestCost = populationDNA[i].TotalCost;
                    BestDNA = populationDNA[i];
                }
                if (populationDNA[i].TotalCost > WorstCost)
                {
                    WorstCost = populationDNA[i].TotalCost;
                    WorstDNA = populationDNA[i];
                }

            }

            Generations++;
        }

        public void GenerateBetter()
        {

            var p = rand.Next((populationDNA.Length - 1)/9);// RandomGeneration.GetRandomNumber(populationDNA.Length - 1);
            if (p%2 != 0) p++;
            var orderedPopulation = GetWorstToBestPopulation(populationDNA);
           
            for (int i = 0; i < p; i++)
            {
                //var replace = populationDNA.Select(x => x.Id == orderedPopulation[i].Id).First();
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
            MaxCost = 0;
            MinCost = double.MaxValue;
            for (int i = 0; i < populationDNA.Length; i++)
            {
                populationDNA[i].CalculateFitness();
                if (populationDNA[i].TotalCost > MaxCost)
                {
                    MaxCost = populationDNA[i].TotalCost;
                }
                if (populationDNA[i].TotalCost < MinCost)
                {
                    MinCost = populationDNA[i].TotalCost;
                }

                TotalFitness += populationDNA[i].Fitness;


            }

            BestCost = double.MaxValue;
            WorstCost = 0;
            for (int i = 0; i < populationDNA.Length; i++)
            {
                populationDNA[i].NormalizeFitness(TotalFitness, MaxCost, MinCost);
                if (populationDNA[i].TotalCost < BestCost)
                {
                    BestCost = populationDNA[i].TotalCost;
                    BestDNA = populationDNA[i];
                }
                if (populationDNA[i].TotalCost > WorstCost)
                {
                    WorstCost = populationDNA[i].TotalCost;
                    WorstDNA = populationDNA[i];
                }

            }

            Generations++;
            
            
        }

        private DNA RankBased(DNA[] population)
        {
            //DNA[] newpop = new DNA[population.Length];
            //for (int i = 0; i < population.Length; i++)
            //{
            //    newpop[i] = population[i];
            //}
            var pop = population.OrderBy(x => x.Fitness).ToArray();
            double[] fitnes = new double[pop.Count()];
            double sum = (double)pop.Count() * (pop.Count() + 1)/2;
            for (int i = 0; i < population.Length; i++)
            {
                fitnes[i] = Convert.ToDouble((i + 1)/sum);
            }

            var index = 0;
            var r = rand.NextDouble();// RandomGeneration.GetRandomDouble();

            while (r > 0)
            {
                r = r - fitnes[index];
                index++;
            }
            index--;
            return pop[index];
        }

        private DNA PickOne(DNA[] population)
        {
            var index = 0;
            var r = rand.NextDouble();// RandomGeneration.GetRandomDouble();

            while (r > 0)
            {
                r = r - population[index].Fitness;
                index++;
            }
            index--;
            return population[index];
        }
        
        public DNA GetBest()
        {
            double worldRecord = 0;
            int index = 0;

            for (int i = 0; i < populationDNA.Length; i++)
            {
                if (populationDNA[i].Fitness > worldRecord)
                {
                    index = i;
                    worldRecord = populationDNA[i].Fitness;
                }
            }
            if (Generations >= _maxGeneration ) finished = true;
            return populationDNA[index];
        }

        public DNA[] GetWorstToBestPopulation(DNA[] population)
        {
            //DNA[] newpop = new DNA[population.Length];
            //for (int i = 0; i < population.Length; i++)
            //{
            //    newpop[i] = population[i];
            //}
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

        // Compute average fitness for the population
        public double GetAverageFitness()
        {
            double total = 0;
            for (int i = 0; i < populationDNA.Length; i++)
            {
                total += populationDNA[i].Fitness;
            }
            return total / (populationDNA.Length);
        }
    }
}
