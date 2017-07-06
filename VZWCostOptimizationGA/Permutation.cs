using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace VZWCostOptimizationGA
{
    public class Permutation
    {
        private List<List<int>> permutatedList = new List<List<int>>();
        public void swapTwoNumber(ref int a, ref int b)
        {
            int temp = a;
            a = b;
            b = temp;
        }
        public List<List<int>> prnPermut(int[] list, int k, int m)
        {
            int i;
            if (k == m)
            {
                List<int> lst = new List<int>();
                for (i = 0; i <= m; i++)
                    lst.Add(list[i]);
                permutatedList.Add(lst);
            }
            else
                for (i = k; i <= m; i++)
                {
                    swapTwoNumber(ref list[k], ref list[i]);
                    prnPermut(list, k + 1, m);
                    swapTwoNumber(ref list[k], ref list[i]);
                }
            return permutatedList;
        }
    }
}
