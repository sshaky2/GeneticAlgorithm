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
        private bool finished;
        private double[] _usage;
        private double perfectScore;
        private int _maxGeneration;
        private string target = "Based on fitness, each member will get added to the mating pool a certain number of times";

        public Population(double mutationRate, int popNumber, int planNum, int maxGeneration, double[] usage )
        {
            _mutationRate = mutationRate;
            population = new DNA[popNumber];
            _usage = usage;

            MaxCost = 0;
            MinCost = double.MaxValue;
            TotalFitness = 0;
            for (int i = 0; i < population.Length; i++)
            {
                population[i] = new DNA(_usage.Length, planNum, _usage);
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
            for (int i = 0; i < population.Length; i++)
            {
                population[i].NormalizeFitness(TotalFitness, MaxCost, MinCost);
            }
            finished = false;
            Generations = 0;
            perfectScore = 1;
            _maxGeneration = maxGeneration;
            
        }


        public void Generate()
        {
            double maxFitness = 0;
           
            for (int i = 0; i < population.Length; i++)
            {
                if (population[i].Fitness > maxFitness)
                {
                    maxFitness = population[i].Fitness;
                }
               
            }

            DNA[] newPopulation = new DNA[population.Length];
            // Refill the population with children from the mating pool
            for (int i = 0; i < population.Length; i++)
            {

                var partnerA = AcceptReject(maxFitness);
                var partnerB = AcceptReject(maxFitness);
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
            for (int i = 0; i < newPopulation.Length; i++)
            {
                population[i].NormalizeFitness(TotalFitness, MaxCost, MinCost);
            }

            Generations++;
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
