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
        private DNA[] population;
        public int Generations { get; set; }
        public double MinCost { get; set; }
        public double MaxCost { get; set; }
        public double TotalFitness { get; set; }
        public double BestCost { get; set; }
        private bool finished;
        private double[] _usage;
        private double perfectScore;
        private int _maxGeneration;

        public Plan[] PlansInfo { get; set; }

        public Population(double mutationRate, int popNumber, int planNum, int maxGeneration, double usageAverage, double[] usage )
        {
            _mutationRate = mutationRate;
            population = new DNA[popNumber];
            _usage = usage;
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
            for (int i = 0; i < population.Length; i++)
            {
                population[i] = new DNA(_usage.Length, planNum, PlansInfo, _usage);
                population[i].CalculateFitness();
                //population[i] = new DNA(target.Length, planNum, _usage);
                if (population[i].TotalCost > MaxCost)
                {
                    MaxCost = population[i].TotalCost;
                }
                if (population[i].TotalCost < MinCost)
                {
                    MinCost = population[i].TotalCost;
                }

                TotalFitness += population[i].Fitness;
            }
            BestCost = Double.MaxValue;
            for (int i = 0; i < population.Length; i++)
            {
                population[i].NormalizeFitness(TotalFitness, MaxCost, MinCost);
                if (population[i].TotalCost < BestCost)
                {
                    BestCost = population[i].TotalCost;
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
            //double maxFitness = 0;
           
            //for (int i = 0; i < population.Length; i++)
            //{
            //    if (population[i].Fitness > maxFitness)
            //    {
            //        maxFitness = population[i].Fitness;
            //    }
               
            //}

            DNA[] newPopulation = new DNA[population.Length];
            for (int i = 0; i < population.Length; i++)
            {

                //var partnerA = AcceptReject(maxFitness);
                //var partnerB = AcceptReject(maxFitness);
                var partnerA = PickOne(population);
                var partnerB = PickOne(population);
                DNA child = partnerA.CrossOver(partnerB);
                child.Mutate(_mutationRate);
                newPopulation[i] = child;
            }
            TotalFitness = 0;
            MaxCost = 0;
            MinCost = double.MaxValue;
            for (int i = 0; i < newPopulation.Length; i++)
            {
                population[i] = newPopulation[i];
                population[i].CalculateFitness();
                if (population[i].TotalCost > MaxCost)
                {
                    MaxCost = population[i].TotalCost;
                }
                if (population[i].TotalCost < MinCost)
                {
                    MinCost = population[i].TotalCost;
                }

                TotalFitness += population[i].Fitness;
               

            }
            BestCost = double.MaxValue;
            for (int i = 0; i < newPopulation.Length; i++)
            {
                population[i].NormalizeFitness(TotalFitness, MaxCost, MinCost);
                if (population[i].TotalCost < BestCost)
                {
                    BestCost = population[i].TotalCost;
                }

            }

            Generations++;
        }

        private DNA PickOne(DNA[] population)
        {
            var index = 0;
            var r = RandomGeneration.GetRandomDouble();

            while (r > 0)
            {
                r = r - population[index].Fitness;
                index++;
            }
            index--;
            return population[index];
        }

        private DNA BetterPickOne(DNA[] population)
        {
            for (int i = 0; i < population.Length; i++)
            {
                int select = 0;
                double selector = RandomGeneration.GetRandomDouble();
                while (selector > 0)
                {
                    selector -= population[i].Fitness;//  scores[select];
                    /*scores[] is the table containing the percentage of selection of each element,
                    for example, if element 3 has a 12 percent chance of being selected, scores[3] = 0.12*/
                    select += 1;
                }
                select -= 1;
                //Here, add element at index select to the new population
                return population[i];
            }
            return null;
        }

        DNA AcceptReject(double maxFitness)
        {
            while (true)
            {
                var index = RandomGeneration.GetRandomNumber(population.Length);
                var partner = population[index];
                double r = 0;
                r = RandomGeneration.GetRandomDouble() * maxFitness;
                   
                if (r < partner.Fitness)
                {
                    return partner;
                }
            }
        }

        public DNA GetBest()
        {
            double worldRecord = 0;
            int index = 0;

            for (int i = 0; i < population.Length; i++)
            {
                if (population[i].Fitness > worldRecord)
                {
                    index = i;
                    worldRecord = population[i].Fitness;
                }
            }
            if (Generations >= _maxGeneration ) finished = true;
            return population[index];
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
            for (int i = 0; i < population.Length; i++)
            {
                total += population[i].Fitness;
            }
            return total / (population.Length);
        }
    }
}
