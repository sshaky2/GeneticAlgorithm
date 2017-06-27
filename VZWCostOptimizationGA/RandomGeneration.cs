using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VZWCostOptimizationGA
{
    public static class RandomGeneration
    {
        private static Random rand = new Random(DateTime.Now.Millisecond);

        public static int GetRandomNumber(int maxValue)
        {
            return rand.Next(maxValue);
        }

        public static double GetRandomDouble()
        {
           return rand.NextDouble();
        }
    }
}
