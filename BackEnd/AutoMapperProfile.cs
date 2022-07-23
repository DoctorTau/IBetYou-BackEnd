using AutoMapper;
using IBUAPI.Models;

namespace BackEnd
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<PostBetDto, Bet>();
        }
    }
}