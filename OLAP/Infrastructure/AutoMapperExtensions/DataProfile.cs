using AutoMapper;
using OLAP.API.Models.Entity.Data;
using OLAP.API.Models.Request.Data;
using OLAP.API.Models.Response.Data;

namespace OLAP.API.Infrastructure.AutoMapperExtensions
{
    public class DataProfile : Profile
    {
        public DataProfile()
        {
            CreateMap<DataEModel, DataReadModel>();
            CreateMap<CountryEModel, CountryReadModel>();
            CreateMap<IndicatorEModel, IndicatorReadModel>();
        }
    }
}
