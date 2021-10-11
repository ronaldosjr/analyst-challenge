using Bogus;
using MongoDB.Bson;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Moq;
using Radix.Domain.Entities;
using Radix.Domain.Interfaces;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;
using System.Linq;
using Radix.Domain.Exceptions;

namespace Radix.Service.Test
{
    public class SensorEventServiceTests
    {
        private readonly IList<SensorEvent> _sensors;
        private readonly Mock<ISensorEventRepository> _mockSensorEventRepository;
        private readonly SensorEventService _sensorEventService;
        private readonly Faker<SensorEvent> _faker;

        public SensorEventServiceTests()
        {
            _mockSensorEventRepository = new Mock<ISensorEventRepository>();
            var regionsSouthEast = new[] { "brasil.sudeste.sensor1", "brasil.sudeste.sensor2", "brasil.sudeste.sensor3" };
            var regionsSouth = new[] { "brasil.sul.sensor1", "brasil.sul.sensor2", "brasil.sul.sensor3" };

            _faker = new Faker<SensorEvent>();
            _faker.RuleFor(x => x.Timestamp, f => new BsonDateTime(f.Date.Recent(1)));
            _faker.RuleFor(x => x.Valor, f => f.Finance.RoutingNumber());
            

            var randomizer = new Randomizer();

            _sensors = Enumerable.Range(0, 3000).Select((x, index) =>
            {
                var sensor = _faker.Generate();
                var position = randomizer.Number(0, 2);
                sensor.Tag = index % 2 == 0 ? regionsSouthEast[position] : regionsSouth[position];
                sensor.Validate();
                return sensor;
            }).ToList();


            _mockSensorEventRepository.Setup(x => x.GetAll()).Returns(_sensors.AsQueryable());
            _mockSensorEventRepository.Setup(x => x.InsertAsync(It.IsAny<SensorEvent>())).Returns((SensorEvent sensor) =>
            {
                sensor.Id = Guid.NewGuid().ToString();
                _sensors.Add(sensor);
                return Task.FromResult(sensor);
            });

            _sensorEventService = new SensorEventService(_mockSensorEventRepository.Object);
            
        }

        [Fact]
        public void ShouldGetAllTest()
        {
            Assert.Equal(_sensors.Count(), _sensorEventService.GetAll().Count());
            Assert.Equal(_sensors.OrderByDescending(x => x.CreationDate).AsQueryable(), _sensorEventService.GetAll());
        }

        [Fact]
        public async Task ShouldInsertTestAsync()
        {
            var sensor = _faker.Generate();
            sensor.Tag = "brasil.sudeste.sensor1";
            await _sensorEventService.InsertAsync(sensor);

            Assert.NotNull(sensor.Id);
            Assert.Contains(_sensors, x => x.Id == sensor.Id);
        }

        [Fact]
        public async Task ShouldNotInsertTestAsync()
        {
            var sensor = _faker.Generate();
            sensor.Tag = "brasil.sudeste";
            
            await Assert.ThrowsAsync<RadixException>(async() => await _sensorEventService.InsertAsync(sensor));

        }


        [Fact]
        public void ShouldGetLatestsTest()
        {
            var latests = _sensorEventService.GetLatests();

            Assert.Equal(_sensors.OrderByDescending(x => x.CreationDate).Take(1000).ToList(), latests);
        }

        [Fact]
        public void ShouldGetAggregateTest()
        {
            var aggregates = _sensorEventService.GetAggregate();

            Assert.Equal(_sensors.Count(x => x.Tag == "brasil.sudeste.sensor1"), aggregates.FirstOrDefault(x => x.Tag == "brasil.sudeste.sensor1").Total);
            Assert.Equal(_sensors.Count(x => x.Tag == "brasil.sudeste.sensor2"), aggregates.FirstOrDefault(x => x.Tag == "brasil.sudeste.sensor2").Total);
            Assert.Equal(_sensors.Count(x => x.Tag == "brasil.sudeste.sensor3"), aggregates.FirstOrDefault(x => x.Tag == "brasil.sudeste.sensor3").Total);
            Assert.Equal(_sensors.Count(x => x.Tag == "brasil.sul.sensor1"), aggregates.FirstOrDefault(x => x.Tag == "brasil.sul.sensor1").Total);
            Assert.Equal(_sensors.Count(x => x.Tag == "brasil.sul.sensor2"), aggregates.FirstOrDefault(x => x.Tag == "brasil.sul.sensor2").Total);
            Assert.Equal(_sensors.Count(x => x.Tag == "brasil.sul.sensor3"), aggregates.FirstOrDefault(x => x.Tag == "brasil.sul.sensor3").Total);
            Assert.Equal(_sensors.Count(x => x.Tag.Contains("brasil.sudeste")), aggregates.FirstOrDefault(x => x.Tag == "brasil.sudeste").Total);
            Assert.Equal(_sensors.Count(x => x.Tag.Contains("brasil.sul")), aggregates.FirstOrDefault(x => x.Tag == "brasil.sul").Total);
            
        }
    }
}
