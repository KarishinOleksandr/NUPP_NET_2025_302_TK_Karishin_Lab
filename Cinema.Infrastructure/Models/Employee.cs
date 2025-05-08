using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Infrastructure.Models
{
    public class Employee : Person
    {
        public string Position { get; set; } = string.Empty;

        public ICollection<EmployeeTicket> EmployeeTickets { get; set; } = new List<EmployeeTicket>();
    }
}
