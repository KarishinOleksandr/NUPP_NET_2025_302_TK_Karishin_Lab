using Cinema.Infrastructure.Models;

public class Employee
{
    public Guid Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Position { get; set; } = string.Empty;
    public DateTime HireDate { get; set; }

    public ICollection<EmployeeTicket> EmployeeTickets { get; set; } = new List<EmployeeTicket>();
}
