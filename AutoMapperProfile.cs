using AutoMapper;
using BackEnd.Models.Dto;
using IBUAPI.Models;

namespace BackEnd
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreatingBetDto, Bet>();
        }
    }
}