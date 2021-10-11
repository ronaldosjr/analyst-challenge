using AutoMapper;
using MongoDB.Bson;
using Radix.Api.Extension;
using Radix.Api.ViewModel;
using Radix.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Radix.Api.AutoMapperProfile
{
    public class RadixProfile : Profile
    {
        public RadixProfile()
        {

            CreateMap<SensorEventViewModel, SensorEvent>()
                .ForMember(x => x.Timestamp, map => map.MapFrom(x => x.Timestamp.ToBsonDateTime()));

            CreateMap<SensorEvent, SensorEventViewModel>()
                .ForMember(x => x.Timestamp, map => map.MapFrom(x => x.Timestamp.MillisecondsSinceEpoch));

        }
    }
}
