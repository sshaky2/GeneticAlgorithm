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
        const int popMax = 4000;
        const double mutationRate = 0.01;
        const int planNum = 8;
        const int maxGeneration = 100000;
        int[] planMembersCount = new int[planNum];
        double[] planMemberUsageSum = new double[planNum];

        public void Execute()
        {
            File.WriteAllText(@"C:\Users\sshakya\Documents\GitHub\VZWCostOptimizationGA\result.txt", string.Empty);
            List<double> arr = new List<double>();
            string path = @"C:\Users\sshakya\Documents\GitHub\VZWCostOptimizationGA\Data\data_large.txt";
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
            List<double> pop3 = new List<double>();
            List<double> pop4 = new List<double>();

            List<double> pop5 = new List<double>();
            List<double> pop6 = new List<double>();
            List<double> pop7 = new List<double>();
            List<double> pop8 = new List<double>();

            for (int i = 0; i < shuffledArray.Length; i++)
            {
                var r = RandomGeneration.GetRandomNumber(8);
                if (r == 0)
                {
                    pop1.Add(shuffledArray[i]);
                }
                else if(r == 1)
                {
                    pop2.Add(shuffledArray[i]);
                }
                else if (r == 2)
                {
                    pop3.Add(shuffledArray[i]);
                }
                else if (r == 3)
                {
                    pop4.Add(shuffledArray[i]);
                }
                else if (r == 4)
                {
                    pop5.Add(shuffledArray[i]);
                }
                else if (r == 5)
                {
                    pop6.Add(shuffledArray[i]);
                }
                else if (r == 6)
                {
                    pop7.Add(shuffledArray[i]);
                }
                else if (r == 7)
                {
                    pop8.Add(shuffledArray[i]);
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

            double total3 = 0;
            foreach (var val in pop3)
            {
                double value = Convert.ToDouble(val);
                total3 += Convert.ToDouble(val);
            }
            double average3 = total3 / pop3.Count;
            Population population3 = new Population(mutationRate, popMax, planNum, maxGeneration, average3, pop3.ToArray());
            Task.Factory.StartNew(() => ExecuteGA(population3, "Population 3"));

            double total4 = 0;
            foreach (var val in pop4)
            {
                double value = Convert.ToDouble(val);
                total4 += Convert.ToDouble(val);
            }
            double average4 = total4 / pop4.Count;
            Population population4 = new Population(mutationRate, popMax, planNum, maxGeneration, average4, pop4.ToArray());
            Task.Factory.StartNew(() => ExecuteGA(population4, "Population 4"));

            double total5 = 0;
            foreach (var val in pop5)
            {
                double value = Convert.ToDouble(val);
                total5 += Convert.ToDouble(val);

            }
            double average5 = total5 / pop5.Count;
            Population population5 = new Population(mutationRate, popMax, planNum, maxGeneration, average5, pop5.ToArray());
            Task.Factory.StartNew(() => ExecuteGA(population5, "Population 5"));

            double total6 = 0;
            foreach (var val in pop6)
            {
                double value = Convert.ToDouble(val);
                total6 += Convert.ToDouble(val);
            }
            double average6 = total6 / pop6.Count;
            Population population6 = new Population(mutationRate, popMax, planNum, maxGeneration, average6, pop6.ToArray());
            Task.Factory.StartNew(() => ExecuteGA(population6, "Population 6"));

            double total7 = 0;
            foreach (var val in pop7)
            {
                double value = Convert.ToDouble(val);
                total7 += Convert.ToDouble(val);
            }
            double average7 = total7 / pop7.Count;
            Population population7 = new Population(mutationRate, popMax, planNum, maxGeneration, average7, pop7.ToArray());
            Task.Factory.StartNew(() => ExecuteGA(population7, "Population 7"));

            double total8 = 0;
            foreach (var val in pop8)
            {
                double value = Convert.ToDouble(val);
                total8 += Convert.ToDouble(val);
            }
            double average8 = total8 / pop8.Count;
            Population population8 = new Population(mutationRate, popMax, planNum, maxGeneration, average8, pop8.ToArray());
            Task.Factory.StartNew(() => ExecuteGA(population8, "Population 8"));
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
                    File.AppendAllText(@"C:\Users\sshakya\Documents\GitHub\VZWCostOptimizationGA\result.txt", $"{popName} Best Cost: {population.BestCost} Worst Cost: {population.WorstCost} {Environment.NewLine}" + Environment.NewLine);
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
