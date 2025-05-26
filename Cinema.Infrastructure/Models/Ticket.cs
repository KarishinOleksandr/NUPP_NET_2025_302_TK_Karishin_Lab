using System;
using System.ComponentModel.DataAnnotations;

namespace Cinema.Infrastructure.Models
{
    public class Ticket
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        public DateTime ShowTime { get; set; }
        public double Price { get; set; }

        public Guid CustomerId { get; set; }
        public Customer Customer { get; set; }

        public Guid MovieId { get; set; }
        public Movie Movie { get; set; }

        public ICollection<EmployeeTicket> EmployeeTickets { get; set; } = new List<EmployeeTicket>();
    }
}
