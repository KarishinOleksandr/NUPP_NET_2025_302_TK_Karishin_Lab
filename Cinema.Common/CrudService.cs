using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace Cinema.Common
{
    public class CrudService<T> : ICrudService<T> where T : IIdentifiable
    {
        private readonly List<T> _items = new();

        public void Create(T element) => _items.Add(element);

        public T? Read(Guid id) => _items.FirstOrDefault(e => e.Id == id);

        public IEnumerable<T> ReadAll() => _items;

        public void Update(T element)
        {
            var index = _items.FindIndex(e => e.Id == element.Id);
            if (index != -1)
                _items[index] = element;
        }

        public void Remove(T element)
        {
            _items.RemoveAll(e => e.Id == element.Id);
        }


        public void Save(string filePath)
        {
            var json = JsonSerializer.Serialize(_items, new JsonSerializerOptions
            {
                WriteIndented = true
            });
            File.WriteAllText(filePath, json);
        }

        public void Load(string filePath)
        {
            if (!File.Exists(filePath)) return;

            var json = File.ReadAllText(filePath);
            var loaded = JsonSerializer.Deserialize<List<T>>(json);

            if (loaded != null)
            {
                _items.Clear();
                _items.AddRange(loaded);
            }
        }
    }
}
