    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using System.Threading.Tasks;

    namespace Cinema.Infrastructure.Models
    {
        public class EmployeeTicket
        {
            public Guid EmployeeId { get; set; }
            public Employee Employee { get; set; } = null!;

            public Guid TicketId { get; set; }
            public Ticket Ticket { get; set; } = null!;
    }
    }
