using System;
using System.Collections.Generic;
using System.Text;

namespace CSharpSyntax;

public static class NumberUtils
{
    extension(int num)
    {
        public  bool IsEven()
        {
            return num % 2 == 0;
        }
        public bool isOdd()         {
            return num % 2 != 0;
        }
        public bool IsNegative()
        {
            return num < 0;
        }

        public DateTime DaysFromNow()
        {
            return DateTime.Now.AddDays(num);
        }
    }

}
