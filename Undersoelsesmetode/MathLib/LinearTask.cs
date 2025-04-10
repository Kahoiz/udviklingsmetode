using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLib;
public class LinearTask
{
    public static List<int> CalculateLinearFunction(int x)
    {
        // Linear equation: y = ax + b
        int b = 1000;  // base memory usage
        int listSize = x + b;

        List<int> res = new();

        var rand = new Random();

        for(int i = 0; i< listSize; i++)
        {
            res.Add(rand.Next(1, 100));
        }

        return res;
    }
}
