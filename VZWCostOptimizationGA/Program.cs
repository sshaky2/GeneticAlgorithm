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
            int popMax = 500;
            double mutationRate = 0.01;
            int planNum = 8;
            int maxGeneration =100000;

            List<double> arr = new List<double>();
            string path = @"C:\Users\sshakya\Documents\GitHub\VZWCostOptimizationGA\Data\data.txt";
            string[] lines = File.ReadAllLines(path);
            double total = 0;
            foreach (var val in lines)
            {
                arr.Add(Convert.ToDouble(val));
                total += Convert.ToDouble(val);
            }
            double average = total/lines.Length;

            Population population = new Population(mutationRate, popMax, planNum, maxGeneration, average, arr.ToArray());
            

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
                Console.WriteLine($"Generation: {population.Generations}, Worst Cost: {population.WorstCost} Total Cost: {population.BestCost}");
            }
        }
    }
}
