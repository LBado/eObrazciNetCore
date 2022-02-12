using AutoMapper;
using eObrazci.DTO;
using eObrazci.Models;

namespace eObrazci.Helper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Student, StudentDTO>();
            CreateMap<StudentDTO, Student>();
            CreateMap<Naslov, NaslovDTO>();
            CreateMap<NaslovDTO, Naslov>();
            CreateMap<Obrazec, ObrazecDTO>();
            CreateMap<Izpit, IzpitDTO>();
            CreateMap<IzpitDTO, Izpit>();
            CreateMap<User, UserDTO>();
            CreateMap<UserDTO, User>();
        }
    }
}
