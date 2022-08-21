using AutoMapper;
using IBUAPI.Models.Dto;
using IBUAPI.Models;

namespace BackEnd
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreatingBetDto, Bet>();
            CreateMap<Bet, GetBetDto>();
            CreateMap<User, GetUserDto>();
        }
    }
}