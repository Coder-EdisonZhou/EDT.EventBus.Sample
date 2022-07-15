using AutoMapper;

namespace EDT.MSA.Ordering.API.Models
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<OrderDTO, Order>().ReverseMap();
            CreateMap<Order, OrderVO>();
        }
    }
}
