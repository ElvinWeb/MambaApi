﻿using AutoMapper;
using MambaApi.DataAccessLayer;
using MambaApi.DTO.ProfessionDtos;
using MambaApi.DTO.WorkerDtos;
using MambaApi.Entities;
using MambaApi.Helpers;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MambaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;
        public WorkersController(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        [HttpGet("{id}")]
        public async Task<IActionResult> GetOne(int id)
        {
            if (id == null && id <= 0) return NotFound();

            Worker worker = await _context.Workers.Where(worker => worker.IsDeleted == false).FirstOrDefaultAsync(worker => worker.Id == id);

            if (worker == null) return NotFound();

            WorkerGetDto workerGetDto = _mapper.Map<WorkerGetDto>(worker);

            return Ok(workerGetDto);
        }


        [HttpGet("")]
        public async Task<IActionResult> GetAll()
        {
            List<Worker> workers = await _context.Workers.Where(worker => worker.IsDeleted == false).ToListAsync();

            IEnumerable<WorkerGetDto> workerGetDtos = workers.Select(worker => new WorkerGetDto { Id = worker.Id, FullName = worker.FullName, Description = worker.Description, MediaUrl = worker.MediaUrl });


            return Ok(workerGetDtos);
        }

        [HttpPost("")]
        public async Task<IActionResult> Create([FromForm] WorkerCreateDto workerCreateDto)
        {

            Worker worker = _mapper.Map<Worker>(workerCreateDto);

            bool check = true;

            if (workerCreateDto.ProfessionIds != null)
            {
                foreach (int professionId in workerCreateDto.ProfessionIds)
                {
                    if (!_context.WorkerProfessions.Any(profession => profession.Id == professionId))
                    {
                        check = false;
                        break;
                    }
                }
            }

            if (!check)
            {
                if (workerCreateDto.ProfessionIds != null)
                {
                    foreach (int professionId in workerCreateDto.ProfessionIds)
                    {
                        WorkerProfession workerProfession = new WorkerProfession
                        {
                            Worker = worker,
                            ProfessionId = professionId,
                        };

                        _context.WorkerProfessions.Add(workerProfession);
                    }
                }
            }
            else
            {
                return NotFound();

            }

            if (workerCreateDto.ImgFile != null)
            {
                if (workerCreateDto.ImgFile.ContentType != "image/png" && workerCreateDto.ImgFile.ContentType != "image/jpeg")
                {
                    return BadRequest();
                }

                if (workerCreateDto.ImgFile.Length > 1048576)
                {
                    return BadRequest();
                }
            }
            else
            {
                return BadRequest();
            }

            string folder = "Uploads/workers-images";
            string newImgUrl = await Helper.GetFileName(folder, workerCreateDto.ImgFile);


            worker.CreatedDate = DateTime.UtcNow.AddHours(4);
            worker.UpdatedDate = DateTime.UtcNow.AddHours(4);
            worker.DeletedDate = DateTime.UtcNow.AddHours(4);
            worker.ImgUrl = newImgUrl;
            worker.IsDeleted = false;

            await _context.Workers.AddAsync(worker);
            await _context.SaveChangesAsync();

            return StatusCode(201, new { message = "Object yaradildi" });
        }
        [HttpPut("")]
        public async Task<IActionResult> Update([FromForm] WorkerUpdateDto workerUpdateDto)
        {

            Worker worker = await _context.Workers.FirstOrDefaultAsync(worker => worker.Id == workerUpdateDto.Id);

            if (worker == null) return NotFound();

            worker.WorkerProfessions.RemoveAll(wp => !workerUpdateDto.ProfessionIds.Contains(wp.ProfessionId));

            foreach (var professionId in workerUpdateDto.ProfessionIds.Where(pId => !worker.WorkerProfessions.Any(wp => wp.ProfessionId == pId)))
            {
                WorkerProfession workerProfession = new WorkerProfession
                {
                    Worker = worker,
                    ProfessionId = professionId,
                };

                worker.WorkerProfessions.Add(workerProfession);
            }

            if (workerUpdateDto.ImgFile != null)
            {
                if (workerUpdateDto.ImgFile.ContentType != "image/png" && workerUpdateDto.ImgFile.ContentType != "image/jpeg")
                {
                    return BadRequest();
                }

                if (workerUpdateDto.ImgFile.Length > 1048576)
                {
                    return BadRequest();
                }

                string folder = "Uploads/workers-images";
                string newImgUrl = await Helper.GetFileName(folder, workerUpdateDto.ImgFile);

                string oldFilePath = Path.Combine(folder, worker.ImgUrl);

                if (System.IO.File.Exists(oldFilePath))
                {
                    System.IO.File.Delete(oldFilePath);
                }

                worker.ImgUrl = newImgUrl;

            }

            worker = _mapper.Map(workerUpdateDto, worker);
            worker.IsDeleted = !worker.IsDeleted;
            worker.UpdatedDate = DateTime.UtcNow.AddHours(4);

            await _context.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("/workers/toggleDelete/{id}")]
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