using AutoMapper;
using TeeScore.Domain;
using TeeScore.DTO;

namespace TeeScore.Mapping
{
    public class DefaultProfile: Profile
    {
        public DefaultProfile()
        {
            CreateMap<Player, PlayerDto>();
            CreateMap<PlayerDto, Player>();
            CreateMap<Venue, VenueDto>();
            CreateMap<VenueDto, Venue>();
            CreateMap<TeeDto, Tee>();
            CreateMap<Tee, TeeDto>();
            CreateMap<ScoreDto, Score>();
            CreateMap<Score, ScoreDto>();
        }


    }
}
