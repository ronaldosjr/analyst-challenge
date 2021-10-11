using MongoDB.Driver;
using System;
using System.Collections.Generic;
using System.Text;

namespace Radix.Infra.Data.Interfaces
{
    public interface IMongoConnect
    {
        IMongoDatabase DataBase { get; }
    }
}
