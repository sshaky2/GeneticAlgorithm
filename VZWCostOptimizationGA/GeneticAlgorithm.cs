using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VZWCostOptimizationGA
{
    public class GeneticAlgorithm
    {
        const int popMax = 1000;
        const double mutationRate = 0.01;
        const int planNum = 8;
        const int maxGeneration = 100000;
        int[] planMembersCount = new int[planNum];
        double[] planMemberUsageSum = new double[planNum];

        public void Execute()
        {
            List<double> arr = new List<double>();
            string path = @"C:\Users\sshakya\Documents\GitHub\VZWCostOptimizationGA\Data\data.txt";
            string[] lines = File.ReadAllLines(path);
            double total = 0;
            foreach (var val in lines)
            {
                double value = Convert.ToDouble(val);
                arr.Add(value);
                total += Convert.ToDouble(val);

            }

            double average = total / lines.Length;
            var shuffledArray = Shuffle(arr);

            List<double> pop1 = new List<double>();
            List<double> pop2 = new List<double>();

            for (int i = 0; i < shuffledArray.Length; i++)
            {
                if (RandomGeneration.GetRandomNumber(2) == 0)
                {
                    pop1.Add(shuffledArray[i]);
                }
                else
                {
                    pop2.Add(shuffledArray[i]);
                }
            }

            double total1 = 0;
            foreach (var val in pop1)
            {
                double value = Convert.ToDouble(val);
                total1 += Convert.ToDouble(val);

            }
            double average1 = total1 / pop1.Count;
            Population population1 = new Population(mutationRate, popMax, planNum, maxGeneration, average1, pop1.ToArray());
            //ExecuteGA(pop1, average1);
            Task.Factory.StartNew(() => ExecuteGA(population1, "Population 1"));

            double total2 = 0;
            foreach (var val in pop2)
            {
                double value = Convert.ToDouble(val);
                total2 += Convert.ToDouble(val);
            }
            double average2 = total2 / pop2.Count;
            Population population2 = new Population(mutationRate, popMax, planNum, maxGeneration, average2, pop2.ToArray());
            Task.Factory.StartNew(() => ExecuteGA(population2, "Population 2"));
        }

        private void ExecuteGA(Population population, string popName)
        {
            //Population population = new Population(mutationRate, popMax, planNum, maxGeneration, average, pop.ToArray());

            int counter1 = 0;
            double lastBestValue = 0;
            while (!population.Finished())
            {
                if (counter1 == 500)
                {
                    string path = @"C:\Users\sshakya\Documents\GitHub\VZWCostOptimizationGA\result.txt";
                    File.WriteAllText(path, String.Empty);
                    File.AppendAllText(path, $"{popName} Best Cost: {population.BestCost} Worst Cost: {population.WorstCost} {Environment.NewLine}" + Environment.NewLine);
                    break;
                }
                //population.NaturalSelection();
                //Create next generation
                population.GenerateBetter();
                //var bestDna = population1.GetBest();

                //for (int i = 0; i < bestDna.Genes.Length; i++)
                //{
                //    Console.Write(bestDna.Genes[i]);
                //}
                //Console.WriteLine();
                Console.WriteLine($"{popName}, Generation: {population.Generations}, Worst Cost: {population.WorstCost} Best Cost: {population.BestCost}");
                if ((int)lastBestValue != (int)population.BestCost)
                {
                    counter1 = 0;
                }
                lastBestValue = population.BestCost;
                counter1++;

            }
        }

        private T[] Shuffle<T>(IEnumerable<T> items)
        {
            var result = items.ToArray();
            for (int i = items.Count(); i > 1; i--)
            {
                int j = RandomGeneration.GetRandomNumber(i);
                var t = result[j];
                result[j] = result[i - 1];
                result[i - 1] = t;
            }

            return result;
        }
    }
}
