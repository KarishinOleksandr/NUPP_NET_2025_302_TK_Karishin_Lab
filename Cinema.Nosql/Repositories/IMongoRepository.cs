using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cinema.Nosql.Repositories
{
    public interface IMongoRepository<T>
    {
        Task<IEnumerable<T>> GetAllAsync();
        Task<T> GetByIdAsync(string id);
        Task CreateAsync(T item);
        Task UpdateAsync(string id, T item);
        Task DeleteAsync(string id);
    }
}