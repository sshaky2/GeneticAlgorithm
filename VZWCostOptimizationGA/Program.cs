using System;
using System.Collections.Generic;
using System.Diagnostics.Eventing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VZWCostOptimizationGA
{
    class Program
    {
        static int popMax = 10;
        static double mutationRate = 0.01;
        static int planNum = 7;
        static int maxGeneration = 100000;
        static int[] planMembersCount = new int[planNum];
        static double[] planMemberUsageSum = new double[planNum];
        static void Main(string[] args)
        {
            try
            {

                GeneticAlgorithm obj = new GeneticAlgorithm();
                obj.Execute();
                Console.ReadKey();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            //List<double> arr = new List<double>();
            //string path = @"C:\Users\sshakya\Documents\GitHub\VZWCostOptimizationGA\Data\data.txt";
            //string[] lines = File.ReadAllLines(path);
            //double total = 0;
            //foreach (var val in lines)
            //{
            //    double value = Convert.ToDouble(val);
            //    arr.Add(value);
            //    total += Convert.ToDouble(val);
                
            //}
            
            //double average = total/lines.Length;
            //var shuffledArray = Shuffle(arr);

            //List<double> pop1 = new List<double>();
            //List<double> pop2 = new List<double>();

            //for (int i = 0; i < shuffledArray.Length; i++)
            //{
            //    if (RandomGeneration.GetRandomNumber(2) == 0)
            //    {
            //        pop1.Add(shuffledArray[i]);
            //    }
            //    else
            //    {
            //        pop2.Add(shuffledArray[i]);
            //    }
            //}

            //double total1 = 0;
            //foreach (var val in pop1)
            //{
            //    double value = Convert.ToDouble(val);
            //    total1 += Convert.ToDouble(val);

            //}
            //double average1 = total1/pop1.Count;

            //Task.Factory.StartNew(() => ExecuteGA(pop1, average1));
            ////Population population1 = new Population(mutationRate, popMax, planNum, maxGeneration, average1, pop1.ToArray());

            ////int counter1 = 0;
            ////Console.WriteLine("Population 1");
            ////double lastBestValue = 0;
            ////while (!population1.Finished())
            ////{
            ////    if (counter1 == 500) break;
            ////    //population.NaturalSelection();
            ////    //Create next generation
            ////    population1.GenerateBetter();
            ////    //var bestDna = population1.GetBest();
                
            ////    //for (int i = 0; i < bestDna.Genes.Length; i++)
            ////    //{
            ////    //    Console.Write(bestDna.Genes[i]);
            ////    //}
            ////    //Console.WriteLine();
            ////    Console.WriteLine($"Population 1, Generation: {population1.Generations}, Worst Cost: {population1.WorstCost} Best Cost: {population1.BestCost}");
            ////    if ((int)lastBestValue != (int)population1.BestCost)
            ////    {
            ////        counter1 = 0;
            ////    }
            ////    lastBestValue = population1.BestCost;
            ////    counter1++;

            ////}
            
            //double total2 = 0;
            //foreach (var val in pop2)
            //{
            //    double value = Convert.ToDouble(val);
            //    total2 += Convert.ToDouble(val);
            //}
            //double average2 = total2 / pop2.Count;

            //Task.Factory.StartNew(() => ExecuteGA(pop2, average2));
            ////Population population2 = new Population(mutationRate, popMax, planNum, maxGeneration, average2, pop2.ToArray());


            ////int counter2 = 0;
            ////Console.WriteLine("Population 2");
            ////while (!population2.Finished())
            ////{
            ////    if (counter2 == 500) break;
            ////    //population.NaturalSelection();
            ////    //Create next generation
            ////    population2.GenerateBetter();
            ////    //var bestDna = population2.GetBest();
            ////    //for (int i = 0; i < bestDna.Genes.Length; i++)
            ////    //{
            ////    //    Console.Write(bestDna.Genes[i]);
            ////    //}
            ////    //Console.WriteLine();
            ////    Console.WriteLine($"Population 2, Generation: {population2.Generations}, Worst Cost: {population2.WorstCost} Best Cost: {population2.BestCost}");
            ////    if ((int)lastBestValue != (int)population2.BestCost)
            ////    {
            ////        counter2 = 0;
            ////    }
            ////    lastBestValue = population2.BestCost;
            ////    counter2++;
            ////}
        }

        private static void ExecuteGA(List<double> pop , double average)
        {
            Population population = new Population(mutationRate, popMax, planNum, maxGeneration, average, pop.ToArray());

            int counter1 = 0;
            Console.WriteLine("Population 1");
            double lastBestValue = 0;
            while (!population.Finished())
            {
                if (counter1 == 500) break;
                //population.NaturalSelection();
                //Create next generation
                population.GenerateBetter();
                //var bestDna = population1.GetBest();

                //for (int i = 0; i < bestDna.Genes.Length; i++)
                //{
                //    Console.Write(bestDna.Genes[i]);
                //}
                //Console.WriteLine();
                Console.WriteLine($"Population 1, Generation: {population.Generations}, Worst Cost: {population.WorstCost} Best Cost: {population.BestCost}");
                if ((int)lastBestValue != (int)population.BestCost)
                {
                    counter1 = 0;
                }
                lastBestValue = population.BestCost;
                counter1++;

            }
        }
        
        public static T[] Shuffle<T>(IEnumerable<T> items)
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
