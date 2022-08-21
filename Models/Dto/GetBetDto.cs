using IBUAPI.Models.Dto;

namespace IBUAPI.Models.Dto
{
    public class GetBetDto : Bet
    {
        public List<GetUserDto> Participants { get; set; } = new List<GetUserDto>();
    }
}