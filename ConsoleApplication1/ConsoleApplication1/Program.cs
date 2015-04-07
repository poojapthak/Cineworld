using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ConsoleApplication1
{
    class Program
    {
        static void Main(string[] args)
        {
            var earliestFriday = GetEarliestFriday();

            var a = Films.SendRequest(key: "Bbc6hz2P", full: true, cinema: 23, dates: earliestFriday);
        }

        private static DateTime GetEarliestFriday()
        {
            var today = DateTime.Now.Date;

            return (from i in Enumerable.Range(0, 7)
                    let dt = today.AddDays(i)
                    where dt.DayOfWeek == DayOfWeek.Friday
                    select dt).Single();
        }
    }
}
