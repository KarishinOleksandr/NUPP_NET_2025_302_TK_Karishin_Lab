public interface IIdentifiable1
{
    Guid Id { get; set; }
}
namespace lab2rework.Services
{
    public class InMemoryCrudService1<T> : ICrudServiceAsync1<T> where T : IIdentifiable1
    {
        private readonly ConcurrentDictionary<Guid, T> _store = new ConcurrentDictionary<Guid, T>();
        private readonly string _filePath;
        private readonly object _lock = new object();

        public InMemoryCrudService1(string filePath)
        {
            _filePath = filePath;
        }

        public Task<bool> CreateAsync1(T element)
        {
            return Task.FromResult(_store.TryAdd(element.Id, element));
        }

        public Task<T> ReadAsync1(Guid id)
        {
            T item;
            _store.TryGetValue(id, out item);
            return Task.FromResult(item);
        }

        public Task<IEnumerable<T>> ReadAllAsync1()
        {
            return Task.FromResult(_store.Values.AsEnumerable());
        }

        public Task<IEnumerable<T>> ReadAllAsync1(int page, int amount)
        {
            return Task.FromResult(_store.Values.Skip((page - 1) * amount).Take(amount));
        }

        public Task<bool> UpdateAsync1(T element)
        {
            _store[element.Id] = element;
            return Task.FromResult(true);
        }

        public Task<bool> RemoveAsync1(T element)
        {
            T removed;
            return Task.FromResult(_store.TryRemove(element.Id, out removed));
        }

        public Task<bool> SaveAsync1()
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
