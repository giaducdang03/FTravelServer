using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FTravel.Service.Utils
{
    public class NumberUtils
    {
        public static int GenerateSixDigitNumber()
        {
            Random random = new Random();
            int number = random.Next(100000, 1000000);
            return number;
        }
    }
}
