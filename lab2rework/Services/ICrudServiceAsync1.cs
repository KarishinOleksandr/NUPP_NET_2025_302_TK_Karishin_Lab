using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace lab2rework.Services
{
    public interface ICrudServiceAsync1<T> : IEnumerable<T>
    {
        Task<bool> CreateAsync1(T element);
        Task<T> ReadAsync1(Guid id);
        Task<IEnumerable<T>> ReadAllAsync1();
        Task<IEnumerable<T>> ReadAllAsync1(int page, int amount);
        Task<bool> UpdateAsync1(T element);
        Task<bool> RemoveAsync1(T element);
        Task<bool> SaveAsync1();
    }
}
