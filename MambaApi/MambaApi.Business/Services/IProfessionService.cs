using MambaApi.Business.DTO.ProfessionDtos;
using MambaApi.Core.Entities;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MambaApi.Business.Services
{
    public interface IProfessionService
    {
        Task CreateAsync([FromForm] ProfessionCreateDto professionCreateDto);
        Task Delete(int id);
        Task ToggleDelete(int id);
        IQueryable<Profession> GetProfessionTable();
        Task<ProfessionGetDto> GetByIdAsync(int id);
        Task<IEnumerable<ProfessionGetDto>> GetAllAsync(string? input);
        Task UpdateAsync([FromForm] ProfessionUpdateDto professionUpdateDto);
    }
}
