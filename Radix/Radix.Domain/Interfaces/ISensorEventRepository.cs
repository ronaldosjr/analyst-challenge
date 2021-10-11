using MongoDB.Driver.Linq;
using Radix.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Radix.Domain.Interfaces
{
    public interface ISensorEventRepository
    {
        IQueryable<SensorEvent> GetAll();
        Task<SensorEvent> InsertAsync(SensorEvent sensor);
    }
}
