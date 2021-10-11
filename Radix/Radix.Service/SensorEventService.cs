using MongoDB.Driver;
using MongoDB.Driver.Linq;
using Radix.Domain.Entities;
using Radix.Domain.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radix.Service
{
    public class SensorEventService : ISensorEventService
    {
        private readonly ISensorEventRepository _repository;
        public SensorEventService(ISensorEventRepository repository)
        {
            _repository = repository;
        }

        public IQueryable<SensorEvent> GetAll()
        {
            return _repository.GetAll().OrderByDescending(x => x.CreationDate);
        }

        public async Task<SensorEvent> InsertAsync(SensorEvent entity)
        {
            entity.Validate();
            return await _repository.InsertAsync(entity);
        }

        public IList<SensorEvent> GetLatests()
        {
            return GetAll().Take(1000).ToList();
        }

        public IList<SensorEventAggregate> GetAggregate()
        {
            var total = _repository.GetAll().Where(x => x.Valor != null && x.Valor != string.Empty).GroupBy(x => x.Tag).Select(x =>
                new SensorEventAggregate
                {
                    Tag = x.Key,
                    Total = x.Count()
                }).ToList();

            total.AddRange(GetRegionAggregate(total));
 
            return total.OrderBy(x => x.Tag).ToList();
        }

        private IList<SensorEventAggregate> GetRegionAggregate(IList<SensorEventAggregate> events)
        {
            return events.Select(x => 
                new SensorEventAggregate
                {
                    Tag = x.Tag.TrimEnd('.').Remove(x.Tag.LastIndexOf('.')),
                    Total = x.Total
                })
                .GroupBy(x => x.Tag).Select(x => 
                new SensorEventAggregate
                {
                    Tag = x.Key,
                    Total = x.Sum(y => y.Total)
                }).ToList();
        }

       
    }
}
