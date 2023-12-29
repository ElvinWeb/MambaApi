using AutoMapper;
using MambaApi.Business.CustomExceptions.Common;
using MambaApi.Business.DTO.ProfessionDtos;
using MambaApi.Core.Entities;
using MambaApi.Core.Repositories;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Server.IIS.Core;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MambaApi.Business.Services.Implementations
{
    public class ProfessionService : IProfessionService
    {
        private readonly IProfessionRepository _professionRepository;
        private readonly IMapper _mapper;

        public ProfessionService(IProfessionRepository professionRepository, IMapper mapper)
        {
            _professionRepository = professionRepository;
            _mapper = mapper;
        }
        public async Task CreateAsync([FromForm] ProfessionCreateDto professionCreateDto)
        {
            Profession profession = _mapper.Map<Profession>(professionCreateDto);

            profession.CreatedDate = DateTime.UtcNow.AddHours(4);
            profession.UpdatedDate = DateTime.UtcNow.AddHours(4);
            profession.DeletedDate = DateTime.UtcNow.AddHours(4);
            profession.IsDeleted = false;

            await _professionRepository.CreateAsync(profession);
            await _professionRepository.CommitChanges();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<ProfessionGetDto>> GetAllAsync(string? input)
        {
            IQueryable<Profession> professions = _professionRepository.GetAllAsyncAsQueryable(profession => profession.IsDeleted == false);

            if (professions is not null)
            {
                if (input is not null)
                {
                    professions = professions.Where(profession => profession.Name.ToLower().Contains(input.ToLower()));
                }
            }

            IEnumerable<ProfessionGetDto> professionGetDtos = professions.Select(profession => new ProfessionGetDto { Id = profession.Id, Name = profession.Name });

            return professionGetDtos;
        }

        public async Task<ProfessionGetDto> GetByIdAsync(int id)
        {

            Profession profession = await _professionRepository.GetByIdAsync(profession => profession.Id == id && profession.IsDeleted == false);

            if (profession == null) throw new notFound("profession couldn't be null!");

            ProfessionGetDto professionGetDto = _mapper.Map<ProfessionGetDto>(profession);

            return professionGetDto;
        }

        public IQueryable<Profession> GetProfessionTable()
        {
            var query = _professionRepository.Table.AsQueryable();

            return query;
        }

        public async Task ToggleDelete(int id)
        {

            Profession profession = await _professionRepository.GetByIdAsync(profession => profession.Id == id);

            if (profession == null) throw new notFound("profession couldn't be null!");

            profession.IsDeleted = !profession.IsDeleted;
            profession.DeletedDate = DateTime.UtcNow.AddHours(4);

            await _professionRepository.CommitChanges();
        }

        public async Task UpdateAsync([FromForm] ProfessionUpdateDto professionUpdateDto)
        {

            Profession profession = await _professionRepository.GetByIdAsync(profession => profession.Id == professionUpdateDto.Id);

            if (profession == null) throw new notFound("profession couldn't be null!");

            profession = _mapper.Map(professionUpdateDto, profession);
            profession.UpdatedDate = DateTime.UtcNow.AddHours(4);

            await _professionRepository.CommitChanges();
        }
    }
}
