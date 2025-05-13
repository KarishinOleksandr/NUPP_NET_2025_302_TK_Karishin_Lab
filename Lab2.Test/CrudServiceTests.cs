using lab2rework.Models;
using lab2rework.Services;
using Xunit;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace Lab2.Tests
{
    public class CrudServiceTests
    {
        private InMemoryCrudService1<Bus1> CreateService()
            => new InMemoryCrudService1<Bus1>("test_buses.json");

        [Fact]
        public async Task CreateAsync_ShouldAddElement()
        {
            var service = CreateService();
            var bus = Bus1.CreateNew();

            var result = await service.CreateAsync1(bus);

            Assert.True(result);
            var found = await service.ReadAsync1(bus.Id);
            Assert.NotNull(found);
            Assert.Equal(bus.Id, found.Id);
        }

        [Fact]
        public async Task ReadAsync_ShouldReturnCorrectElement()
        {
            var service = CreateService();
            var bus = Bus1.CreateNew();

            await service.CreateAsync1(bus);
            var found = await service.ReadAsync1(bus.Id);

            Assert.NotNull(found);
            Assert.Equal(bus.Id, found.Id);
        }

        [Fact]
        public async Task ReadAsync_ShouldReturnNullForNonExistentElement()
        {
            var service = CreateService();
            var found = await service.ReadAsync1(Guid.NewGuid());

            Assert.Null(found);
        }

        [Fact]
        public async Task ReadAllAsync_ShouldReturnElements()
        {
            var service = CreateService();

            await service.CreateAsync1(Bus1.CreateNew());
            await service.CreateAsync1(Bus1.CreateNew());

            var all = await service.ReadAllAsync1();

            Assert.NotEmpty(all);
        }

        [Fact]
        public async Task ReadAllAsync_ShouldReturnPaginatedResults()
        {
            var service = CreateService();
            var buses = Enumerable.Range(0, 5).Select(_ => Bus1.CreateNew()).ToList();

            foreach (var bus in buses) await service.CreateAsync1(bus);

            var page = await service.ReadAllAsync1(page: 1, amount: 3);

            Assert.Equal(3, page.Count());
        }

        [Fact]
        public async Task UpdateAsync_ShouldModifyElement()
        {
            var service = CreateService();
            var bus = Bus1.CreateNew();

            await service.CreateAsync1(bus);
            bus.Model1 = "UpdatedModel";

            var result = await service.UpdateAsync1(bus);

            Assert.True(result);
            var updatedBus = await service.ReadAsync1(bus.Id);
            Assert.Equal("UpdatedModel", updatedBus.Model1);
        }

        [Fact]
        public async Task RemoveAsync_ShouldDeleteElement()
        {
            var service = CreateService();
            var bus = Bus1.CreateNew();

            await service.CreateAsync1(bus);
            var result = await service.RemoveAsync1(bus);

            Assert.True(result);
            var found = await service.ReadAsync1(bus.Id);
            Assert.Null(found);
        }

        [Fact]
        public async Task SaveAsync_ShouldPersistData()
        {
            var service = CreateService();
            await service.CreateAsync1(Bus1.CreateNew());

            var result = await service.SaveAsync1();

            Assert.True(result);
        }
    }
}