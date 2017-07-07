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
        List<int> _targetAverages = new List<int> {10240, 5120, 1024,250, 25, 3 };

        private Random rand;

        public GeneticAlgorithm()
        {
            rand = new Random(DateTime.Now.Millisecond);
        }

        public void Execute()
        {
            File.WriteAllText(@"C:\Users\sshakya\Documents\GitHub\VZWCostOptimizationGA\result.txt", string.Empty);
            List<Tuple<long, double>> arr = new List<Tuple<long, double>> ();
            string path = @"C:\Users\sshakya\Documents\GitHub\VZWCostOptimizationGA\Data\data_with_sim.txt";
            string[] lines = File.ReadAllLines(path);
            double total = 0;
            foreach (var val in lines)
            {
                string[] line = val.Split(' ');
                arr.Add(new Tuple<long, double>( Convert.ToInt64(line[0]), Convert.ToDouble(line[2])));
                total += Convert.ToDouble(Convert.ToDouble(line[2]));

            }

            double average = total / lines.Length;
            

            Permutation perm = new Permutation();
            var permutedPlans = perm.prnPermut(_targetAverages.ToArray(), 0, _targetAverages.Count - 1);

            int setCount = 0;
            //foreach (var item in permutedPlans)
            {
                var shuffledArray = Shuffle(arr);
                var item = new List<int> {10240,5120,1024,3,250,25};
                double cost = 0;
                for (int i = 0; i < item.Count; i++)
                {
                    int lastArraySize = -1;
                    File.AppendAllText(@"C:\Users\sshakya\Documents\GitHub\VZWCostOptimizationGA\result.txt", $"Target average {item[i]} {Environment.NewLine}");
                    do
                    {
                        Population population = new Population(mutationRate, popMax, item[i],
                        shuffledArray.ToArray());
                        lastArraySize = shuffledArray.Length;
                        var selectedMembers = ExecuteGA(population);
                        List<int> removeIndex = new List<int>();
                        int count = 0;
                        double usageSum = 0;
                        for (int j = 0; j < selectedMembers.Length; j++)
                        {
                            if (selectedMembers[j] == 1)
                            {
                                removeIndex.Add(j);
                                count++;
                                usageSum += shuffledArray[i].Item2;
                            }

                        }
                        var planInformation = PlanInformation.GetInfo(item[i]);
                        cost += count * planInformation.Cost;
                        if (usageSum > count * planInformation.Size)
                        {
                            cost += (usageSum - (count * planInformation.Size)) * planInformation.OverageCost;
                        }
                        var abc = shuffledArray.ToList();
                        abc.RemoveAll(x => removeIndex.Contains(abc.IndexOf(x)));
                        shuffledArray = abc.ToArray();
                        
                        
                    } while (shuffledArray.Length != lastArraySize);
                    setCount++;
                }
                File.AppendAllText(@"C:\Users\sshakya\Documents\GitHub\VZWCostOptimizationGA\result.txt", $"Set {setCount}:  Total Cost: {cost} {Environment.NewLine} Remaining items: {Environment.NewLine}");
                foreach (var x in shuffledArray)
                {
                    File.AppendAllText(@"C:\Users\sshakya\Documents\GitHub\VZWCostOptimizationGA\result.txt", $"{x} {Environment.NewLine}");
                    Console.WriteLine($"Remaining: {x}");
                }
                File.AppendAllText(@"C:\Users\sshakya\Documents\GitHub\VZWCostOptimizationGA\result.txt", $"{Environment.NewLine}");
            }


        }

       

        private int[] ExecuteGA(Population population)
        {

            int counter = 0;
            double lastBestValue = 0;
            double bestAverage = 0;
            while (true)
            {
                bestAverage = population.BestDNA.GeneAverage;

                if(counter >= 100)
                {
                    if((population.TargetAverage - bestAverage) > 0 && (population.TargetAverage - bestAverage) < 5)
                    {
                        File.AppendAllText(@"C:\Users\sshakya\Documents\GitHub\VZWCostOptimizationGA\result.txt",
                            $"Best Average: {bestAverage} {Environment.NewLine}");
                        var bestDna = population.BestDNA;
                        break;
                    }
                    else
                    {
                        return new int[population.BestDNA.Genes.Length];
                    }

                }
                population.GenerateBetter();
                Console.WriteLine($"Generation: {population.Generations}, Best Average: {bestAverage}");
                if ((int)lastBestValue != (int)bestAverage)
                {
                    counter = 0;
                }
                lastBestValue = bestAverage;
                counter++;

            }
            return population.BestDNA.Genes;
        }

        private T[] Shuffle<T>(IEnumerable<T> items)
        {
            var result = items.ToArray();
            for (int i = items.Count(); i > 1; i--)
            {
                int j = rand.Next(i);// RandomGeneration.GetRandomNumber(i);
                var t = result[j];
                result[j] = result[i - 1];
                result[i - 1] = t;
            }

            return result;
        }
    }
}
