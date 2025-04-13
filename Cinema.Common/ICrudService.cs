using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace Cinema.Common
{
    public interface ICrudService<T>
    {
        void Create(T element);
        T? Read(Guid id); // Вертає null, якщо не знайдено
        IEnumerable<T> ReadAll();
        void Update(T element);
        void Remove(T element);
    }


}
