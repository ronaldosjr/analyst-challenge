using MongoDB.Driver.Linq;
using Radix.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radix.Domain.Interfaces
{
    public interface ISensorEventService
    {
        Task<SensorEvent> InsertAsync(SensorEvent entity);

        IList<SensorEventAggregate> GetAggregate();

        IQueryable<SensorEvent> GetAll();

        IList<SensorEvent> GetLatests();

    }
}
