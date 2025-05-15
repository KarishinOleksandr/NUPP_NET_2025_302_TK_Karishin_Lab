using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cinema.Nosql.Models;
using Cinema.Nosql.Repositories;

namespace Cinema.Nosql.Services
{
    public class MongoCinemaService
    {
        private readonly IMongoRepository<MongoMovie> _movieRepository;
        private readonly IMongoRepository<MongoTicket> _ticketRepository;
        private readonly IMongoRepository<MongoCustomer> _customerRepository;

        public MongoCinemaService(
            IMongoRepository<MongoMovie> movieRepository,
            IMongoRepository<MongoTicket> ticketRepository,
            IMongoRepository<MongoCustomer> customerRepository)
        {
            _movieRepository = movieRepository;
            _ticketRepository = ticketRepository;
            _customerRepository = customerRepository;
        }

        public async Task AddMovieAsync(MongoMovie movie) => await _movieRepository.CreateAsync(movie);
        public async Task<IEnumerable<MongoMovie>> GetAllMoviesAsync() => await _movieRepository.GetAllAsync();
        public async Task<MongoMovie?> GetMovieAsync(string id) => await _movieRepository.GetByIdAsync(id);

        public async Task AddTicketAsync(MongoTicket ticket) => await _ticketRepository.CreateAsync(ticket);
        public async Task<IEnumerable<MongoTicket>> GetAllTicketsAsync() => await _ticketRepository.GetAllAsync();
        public async Task<IEnumerable<MongoTicket>> GetTicketsForMovieAsync(string movieId)
        {
            var allTickets = await _ticketRepository.GetAllAsync();
            return allTickets.Where(t => t.MovieId == movieId);
        }

        public async Task AddCustomerAsync(MongoCustomer customer) => await _customerRepository.CreateAsync(customer);
        public async Task<MongoCustomer?> GetCustomerAsync(string id) => await _customerRepository.GetByIdAsync(id);
        public async Task AddTicketToCustomerAsync(string customerId, string ticketId)
        {
            var customer = await _customerRepository.GetByIdAsync(customerId);
            if (customer != null && !customer.TicketIds.Contains(ticketId))
            {
                customer.TicketIds.Add(ticketId);
                await _customerRepository.UpdateAsync(customerId, customer);
            }
        }
    }
}