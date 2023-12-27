using AutoMapper;
using MambaApi.Business.DTO.ProfessionDtos;
using MambaApi.Business.DTO.WorkerDtos;
using MambaApi.Core.Entities;

namespace MambaApi.MappingProfile
{
    public class MapProfile : Profile
    {
        public MapProfile()
        {

            CreateMap<ProfessionCreateDto, Profession>().ReverseMap();
            CreateMap<ProfessionGetDto, Profession>().ReverseMap();
            CreateMap<ProfessionUpdateDto, Profession>().ReverseMap();


            CreateMap<WorkerCreateDto, Worker>().ReverseMap();
            CreateMap<WorkerGetDto, Worker>().ReverseMap();
            CreateMap<WorkerUpdateDto, Worker>().ReverseMap();
        }
    }
}
