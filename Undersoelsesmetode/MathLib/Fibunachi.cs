﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MathLib
{
    public class Fibunachi
    {
        public static int GetFibunachi(int n)
        {
            if (n == 0)
                return 0;
            if (n == 1)
                return 1;
            return GetFibunachi(n - 1) + GetFibunachi(n - 2);
        }
    }
}
