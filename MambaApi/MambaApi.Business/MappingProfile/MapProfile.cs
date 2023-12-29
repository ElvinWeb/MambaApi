using AutoMapper;
using MambaApi.Business.DTO.ProfessionDtos;
using MambaApi.Business.DTO.WorkerDtos;
using MambaApi.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MambaApi.Business.MappingProfile
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
