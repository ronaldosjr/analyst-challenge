using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Radix.Domain.Entities;
using Radix.Domain.Interfaces;
using Radix.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radix.Infra.Data.Repository
{
    public class SensorEventRepository : ISensorEventRepository
    {
        private readonly IMongoConnect _mongoConnect;

        public SensorEventRepository(IMongoConnect mongoConnect)
        {
            _mongoConnect = mongoConnect;
        }

        public IQueryable<SensorEvent> GetAll()
        {
            return GetCollection.AsQueryable();
        }

        public async Task<SensorEvent> InsertAsync(SensorEvent sensor)
        {
            await GetCollection.InsertOneAsync(sensor);
            return sensor;
        }

        private IMongoCollection<SensorEvent> GetCollection => _mongoConnect.DataBase.GetCollection<SensorEvent>(Environment.GetEnvironmentVariable("SENSOR_EVENT_COLLETION"));
    }
}
