using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Collections.Concurrent;
using Newtonsoft.Json;
using System.IO;
using Lab2.Interfaces;

namespace Lab2.Services
{
    public class InMemoryCrudService<T> : ICrudServiceAsync<T> where T : IIdentifiable
    {
        private readonly ConcurrentDictionary<Guid, T> _store = new ConcurrentDictionary<Guid, T>();
        private readonly string _filePath;
        private readonly object _lock = new object();

        public InMemoryCrudService(string filePath)
        {
            _filePath = filePath;
        }

        public Task<bool> CreateAsync(T element)
        {
            return Task.FromResult(_store.TryAdd(element.Id, element));
        }

        public Task<T> ReadAsync(Guid id)
        {
            T item;
            _store.TryGetValue(id, out item);
            return Task.FromResult(item);
        }

        public Task<IEnumerable<T>> ReadAllAsync()
        {
            return Task.FromResult(_store.Values.AsEnumerable());
        }

        public Task<IEnumerable<T>> ReadAllAsync(int page, int amount)
        {
            return Task.FromResult(_store.Values.Skip((page - 1) * amount).Take(amount));
        }

        public Task<bool> UpdateAsync(T element)
        {
            _store[element.Id] = element;
            return Task.FromResult(true);
        }

        public Task<bool> RemoveAsync(T element)
        {
            T removed;
            return Task.FromResult(_store.TryRemove(element.Id, out removed));
        }

        public Task<bool> SaveAsync()
        {
            lock (_lock)
            {
                var json = JsonConvert.SerializeObject(_store.Values);
                File.WriteAllText(_filePath, json);
            }
            return Task.FromResult(true);
        }

        public IEnumerator<T> GetEnumerator() => _store.Values.GetEnumerator();
        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
