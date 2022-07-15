using AutoMapper;

namespace EDT.MSA.Stocking.API.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<StockDTO, Stock>().ReverseMap();
            CreateMap<Stock, StockVO>();
        }
    }
}
