using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
namespace Cinema.Infrastructure.Services
{
    public class EfCrudService<T> : ICrudServiceAsync<T> where T : class
    {
        private readonly IRepository<T> _repository;

        public EfCrudService(IRepository<T> repository)
        {
            _repository = repository;
        }

        public async Task<bool> CreateAsync(T element)
        {
            await _repository.AddAsync(element);
            return await _repository.SaveChangesAsync() > 0;
        }

        public async Task<T?> ReadAsync(Guid id) => await _repository.GetByIdAsync(id);

        public async Task<IEnumerable<T>> ReadAllAsync() => await _repository.GetAllAsync();

        public async Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            var allItems = await _repository.GetAllAsync();
            return allItems.Skip((page - 1) * amount).Take(amount);
        }

        public async Task<bool> UpdateAsync(T element)
        {
            await _repository.UpdateAsync(element);
            return await _repository.SaveChangesAsync() > 0;
        }

        public async Task<bool> RemoveAsync(T element)
        {
            await _repository.DeleteAsync(element);
            return await _repository.SaveChangesAsync() > 0;
        }

        public async Task<bool> SaveAsync() => await _repository.SaveChangesAsync() > 0;
    }
}