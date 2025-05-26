namespace Cinema.REST.Models
{
    public class TicketModel
    {
        public Guid MovieId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime ShowTime { get; set; }
        public double Price { get; set; }
    }
}
