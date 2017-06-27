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
        private List<DNA> matingPool;
        public int Generations { get; set; }
        private bool finished;
        private double[] _usage;
        private double perfectScore;
        private int _maxGeneration;
        private string target = "Arbitrary multiplier, we can also use monte carlo method";

        public Population(double mutationRate, int popNumber, int planNum, int maxGeneration, double[] usage )
        {
            _mutationRate = mutationRate;
            population = new DNA[popNumber];
            _usage = usage;

            for (int i = 0; i < population.Length; i++)
            {
                //population[i] = new DNA(_usage.Length, planNum, _usage);
                population[i] = new DNA(target.Length, planNum, _usage);
            }
            CalcFitness();
            matingPool = new List<DNA>();
            finished = false;
            Generations = 0;
            perfectScore = 1;
            _maxGeneration = maxGeneration;
            
        }

        public void CalcFitness()
        {
            for (int i = 0; i < population.Length; i++)
            {
                population[i].CalculateFitness(_usage);
            }
        }

        public void NaturalSelection()
        {
            matingPool.Clear();
            double maxFitness = 0;
            for (int i = 0; i < population.Length; i++)
            {
                if (population[i].Fitness > maxFitness)
                {
                    maxFitness = population[i].Fitness;
                }
            }

            // Based on fitness, each member will get added to the mating pool a certain number of times
            // a higher fitness = more entries to mating pool = more likely to be picked as a parent
            // a lower fitness = fewer entries to mating pool = less likely to be picked as a parent
            for (int i = 0; i < population.Length; i++)
            {

                double fitness = Map(population[i].Fitness, maxFitness);
                int n = (int)(fitness * 100);  // Arbitrary multiplier, we can also use monte carlo method
                for (int j = 0; j < n; j++)
                {              // and pick two random numbers
                    matingPool.Add(population[i]);
                }
            }
        }

        private double Map(double fitness, double maxFitness)
        {
            return (fitness/(maxFitness));
        }

        public void Generate()
        {
            // Refill the population with children from the mating pool
            for (int i = 0; i < population.Length; i++)
            {
                int a = RandomGeneration.GetRandomNumber(matingPool.Count);
                int b = RandomGeneration.GetRandomNumber(matingPool.Count);
                DNA partnerA = matingPool[a];
                DNA partnerB = matingPool[b];
                DNA child = partnerA.CrossOver(partnerB);
                child.Mutate(_mutationRate);
                population[i] = child;
            }
            Generations++;
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
            if (worldRecord == 1 ) finished = true;
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
        double GetAverageFitness()
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
