using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.ComponentModel.DataAnnotations;
using Cinema.Common;


namespace Cinema.Infrastructure.Models
{
    public class Ticket
    {
        [Key]

        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime ShowTime { get; set; }

        public double Price { get; set; }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; } = null;

        public Guid MovieId { get; set; }
        public Movie Movie { get; set; } = null;

        public ICollection<EmployeeTicket> EmployeeTickets { get; set; } = new List<EmployeeTicket>();

    }
}
