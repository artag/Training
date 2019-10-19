using AutoMapper;
using CoreCodeCamp.Models;

namespace CoreCodeCamp.Data
{
    public class CampProfile : Profile
    {
        public CampProfile()
        {
            CreateMap<Camp, CampModel>()
                .ForMember(c => c.Venue, o => o.MapFrom(m => m.Location.VenueName));

            CreateMap<CampModel, Camp>()
                .ForPath(c => c.Location.VenueName, o => o.MapFrom(m => m.Venue));
        }
    }
}
