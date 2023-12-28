using AutoMapper;
using MambaApi.Business.CustomExceptions.Common;
using MambaApi.Business.DTO.WorkerDtos;
using MambaApi.Business.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace MambaApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class WorkersController : ControllerBase
    {
        private readonly IWorkerService _workerService;
        public WorkersController(IWorkerService workerService)
        {
            _workerService = workerService;
        }


        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetOne(int id)
        {

            if (id == null && id <= 0) return NotFound();
            WorkerGetDto workerGetDto = null;
            try
            {
                workerGetDto = await _workerService.GetByIdAsync(id);

            }
            catch (notFound ex)
            {
                return NotFound(ex.Message);
            }


            return Ok(workerGetDto);
        }


        [HttpGet("")]
        public async Task<IActionResult> GetAll(string? input, int? professionId, int? orderId)
        {

            IEnumerable<WorkerGetDto> workerGetDtos = await _workerService.GetAllAsync(input, professionId, orderId);

            return Ok(workerGetDtos);
        }

        [HttpPost("")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        public async Task<IActionResult> Create([FromForm] WorkerCreateDto workerCreateDto)
        {
            try
            {

                await _workerService.CreateAsync(workerCreateDto);
            }
            catch (InvalidImageContentTypeOrSize ex)
            {
                return BadRequest(ex.Message);
            }
            catch (InvalidImage ex)
            {
                return BadRequest(ex.Message);
            }
            catch (notFound ex)
            {
                return NotFound(ex.Message);
            }

            return StatusCode(201, new { message = "Object yaradildi" });
        }
        [HttpPut("")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> Update([FromForm] WorkerUpdateDto workerUpdateDto)
        {
            try
            {

                await _workerService.UpdateAsync(workerUpdateDto);
            }
            catch (InvalidImageContentTypeOrSize ex)
            {
                return BadRequest(ex.Message);
            }
            catch (notFound ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }

        [HttpDelete("/workers/toggleDelete/{id}")]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status204NoContent)]
        public async Task<IActionResult> ToggleDelete(int id)
        {

            if (id == null && id <= 0) return NotFound();

            try
            {

                await _workerService.ToggleDelete(id);
            }
            catch (notFound ex)
            {
                return NotFound(ex.Message);
            }

            return NoContent();
        }
    }
}
