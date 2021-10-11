using MongoDB.Bson;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radix.Api.Extension
{
    public static class StringExtension
    {
        public static BsonDateTime ToBsonDateTime(this string source)
        {
            if (string.IsNullOrWhiteSpace(source))
                return null;

            if (!long.TryParse(source, out var time))
                return null;

            if (time <= 0)
                return null;

            return new BsonDateTime(time);
        }
    }
}
