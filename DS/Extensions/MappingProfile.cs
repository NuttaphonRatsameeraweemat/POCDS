using AutoMapper;
using DS.Bll.Models;
using DS.Data.Pocos;

namespace DS.Extensions
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // Add as many of these lines as you need to map your objects
            CreateMap<Ca, CaViewModel>();
            CreateMap<CaViewModel, Ca>();
        }
    }
}
