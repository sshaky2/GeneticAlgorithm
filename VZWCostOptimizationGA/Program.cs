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
           
        }

    }
}
