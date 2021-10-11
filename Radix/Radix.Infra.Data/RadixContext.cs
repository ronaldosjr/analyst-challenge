using MongoDB.Bson.Serialization;
using MongoDB.Bson.Serialization.Conventions;
using MongoDB.Driver;
using Radix.Domain.Entities;
using Radix.Domain.Interfaces;
using Radix.Infra.Data.Interfaces;
using System;
using System.Collections.Generic;
using System.Text;

namespace Radix.Infra.Data
{
    public class RadixContext : IMongoConnect
    {
        public RadixContext()
        {

            var conventionPack = new ConventionPack { new CamelCaseElementNameConvention() };
            ConventionRegistry.Register("camelCase", conventionPack, t => true);

            var settings = MongoClientSettings.FromConnectionString(Environment.GetEnvironmentVariable("MONGO_CONNECTION"));
            DataBase = new MongoClient(settings).GetDatabase(Environment.GetEnvironmentVariable("MONGO_DATABASE_NAME"));

        }

        public IMongoDatabase DataBase { get; }
    }
}
