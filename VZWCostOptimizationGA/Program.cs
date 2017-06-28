using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VZWCostOptimizationGA
{
    class Program
    {
        static void Main(string[] args)
        {
            int popMax = 100;
            double mutationRate = 0.01;
            int planNum = 8;
            int maxGeneration = 10000;

            List<double> arr = new List<double>();
            string path = @"C:\Users\sshakya\Documents\Visual Studio 2015\Projects\VZWCostOptimizationGA\Data\data.txt";
            string[] lines = File.ReadAllLines(path);
            foreach (var val in lines)
            {
                arr.Add(Convert.ToDouble(val));
            }

            Population population = new Population(mutationRate, popMax, planNum, maxGeneration, arr.ToArray());

            while (!population.Finished())
            {
                //population.NaturalSelection();
                //Create next generation
                population.Generate();
                var bestDna = population.GetBest();
                //for (int i = 0; i < bestDna.Genes.Length; i++)
                //{
                //    Console.Write(bestDna.Genes[i]);
                //}
                //Console.WriteLine();
                Console.WriteLine($"Generation: {population.Generations}, Best fitness: {population.GetBest().Fitness} Average Fitness: {population.GetAverageFitness()} Total Cost: {population.GetBest().TotalCost}");
            }
        }
    }
}
