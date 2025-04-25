using Lab2.Models;
using Lab2.Services;
using Xunit;
using System.Threading.Tasks;

namespace Lab2.Tests
{
    public class CrudServiceTests
    {
        [Fact]
        public async Task CreateAsync_ShouldAddElement()
        {
            var service = new InMemoryCrudService<Bus>("test_buses.json");
            var bus = Bus.CreateNew();

            var result = await service.CreateAsync(bus);

            Assert.True(result);

            var found = await service.ReadAsync(bus.Id);
            Assert.NotNull(found);
            Assert.Equal(bus.Id, found.Id);
        }

        [Fact]
        public async Task RemoveAsync_ShouldRemoveElement()
        {
            var service = new InMemoryCrudService<Bus>("test_buses.json");
            var bus = Bus.CreateNew();

            await service.CreateAsync(bus);
            var result = await service.RemoveAsync(bus);

            Assert.True(result);

            var found = await service.ReadAsync(bus.Id);
            Assert.Null(found);
        }

        [Fact]
        public async Task ReadAllAsync_ShouldReturnElements()
        {
            var service = new InMemoryCrudService<Bus>("test_buses.json");

            await service.CreateAsync(Bus.CreateNew());
            await service.CreateAsync(Bus.CreateNew());

            var all = await service.ReadAllAsync();

            Assert.NotEmpty(all);
        }
    }
}
