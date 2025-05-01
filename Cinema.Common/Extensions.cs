using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Common
{
    public static class Extensions
    {
        public static double CalculateTotalPrice(this List<Ticket> tickets)
        {
            return tickets.Sum(t => t.Price);
        }
    }
}
