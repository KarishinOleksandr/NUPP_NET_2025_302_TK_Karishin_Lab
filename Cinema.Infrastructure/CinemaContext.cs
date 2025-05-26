using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Cinema.Infrastructure.Models;

namespace Cinema.Infrastructure
{
    public class CinemaContext : IdentityDbContext<ApplicationUser>
    {
        public CinemaContext(DbContextOptions<CinemaContext> options) : base(options) { }

        public DbSet<Person> People => Set<Person>();
        public DbSet<Customer> Customers => Set<Customer>();
        public DbSet<Employee> Employees => Set<Employee>();
        public DbSet<Movie> Movies => Set<Movie>();
        public DbSet<Ticket> Tickets => Set<Ticket>();
        public DbSet<EmployeeTicket> EmployeeTickets => Set<EmployeeTicket>();

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<Person>().ToTable("People");
            modelBuilder.Entity<Customer>().ToTable("Customers");
            modelBuilder.Entity<Employee>().ToTable("Employees");

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Customer)
                .WithMany(c => c.Tickets)
                .HasForeignKey(t => t.CustomerId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Ticket>()
                .HasOne(t => t.Movie)
                .WithMany(m => m.Tickets)
                .HasForeignKey(t => t.MovieId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeTicket>()
                .HasKey(et => new { et.EmployeeId, et.TicketId });

            modelBuilder.Entity<EmployeeTicket>()
                .HasOne(et => et.Employee)
                .WithMany(e => e.EmployeeTickets)
                .HasForeignKey(et => et.EmployeeId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<EmployeeTicket>()
                .HasOne(et => et.Ticket)
                .WithMany(t => t.EmployeeTickets)
                .HasForeignKey(et => et.TicketId)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}
