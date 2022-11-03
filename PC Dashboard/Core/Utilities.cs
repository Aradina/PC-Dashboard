using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PC_Dashboard
{
    class Utilities
    {

        public static DateTimeOffset UnixToDateTime(int unixtime)
        {
            DateTimeOffset dt = DateTimeOffset.FromUnixTimeSeconds(unixtime);
            Debug.WriteLine($"DateTimeOffsetFromUnixTimeSeconds: {dt}");
            return dt;
        }


    }
}
