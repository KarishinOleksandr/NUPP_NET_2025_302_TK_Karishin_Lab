using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Infrastructure.Models
{
    public class Customer : Person
    {
        public string FavoriteGenre { get; set; } = string.Empty;

        public ICollection<Ticket> Tickets { get; set; } = new List<Ticket>();
    }
}
