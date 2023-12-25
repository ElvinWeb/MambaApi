using AutoMapper;
using MambaApi.DataAccessLayer;
using MambaApi.DTO.ProfessionDtos;
using MambaApi.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MambaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProfessionsController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public ProfessionsController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public async Task<IActionResult> GetOne(int id)
        {
            if (id == null && id <= 0) return NotFound();

            Profession profession = await _context.Professions.Where(profession => profession.IsDeleted == false).FirstOrDefaultAsync(profession => profession.Id == id);

            if (profession == null) return NotFound();

            ProfessionGetDto professionGetDto = _mapper.Map<ProfessionGetDto>(profession);

            return Ok(professionGetDto);
        }


        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            List<Profession> professions = await _context.Professions.Where(profession => profession.IsDeleted == false).ToListAsync();

            IEnumerable<ProfessionGetDto> professionGetDtos = professions.Select(profession => new ProfessionGetDto { Id = profession.Id, Name = profession.Name });


            return Ok(professionGetDtos);
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromForm] ProfessionCreateDto professionCreateDto)
        {

            Profession profession = _mapper.Map<Profession>(professionCreateDto);

            profession.CreatedDate = DateTime.UtcNow.AddHours(4);
            profession.UpdatedDate = DateTime.UtcNow.AddHours(4);
            profession.DeletedDate = DateTime.UtcNow.AddHours(4);
            profession.IsDeleted = false;

            await _context.Professions.AddAsync(profession);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = "Object yaradildi" });
        }

        [HttpPut("")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update([FromForm] ProfessionUpdateDto professionUpdateDto)
        {

            Profession profession = await _context.Professions.FirstOrDefaultAsync(profession => profession.Id == professionUpdateDto.Id);

            if (profession == null) return NotFound();

            profession = _mapper.Map(professionUpdateDto, profession);

            profession.UpdatedDate = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("/professions/toggleDelete/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ToggleDelete(int id)
        {
            if (id == null && id <= 0) return NotFound();

            Profession profession = await _context.Professions.FirstOrDefaultAsync(profession => profession.Id == id);

            if (profession == null) return NotFound();

            profession.IsDeleted = !profession.IsDeleted;
            profession.DeletedDate = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
