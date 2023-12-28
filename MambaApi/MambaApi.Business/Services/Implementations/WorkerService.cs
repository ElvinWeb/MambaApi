using AutoMapper;
using FluentValidation.Validators;
using MambaApi.Business.CustomExceptions.Common;
using MambaApi.Business.DTO.WorkerDtos;
using MambaApi.Business.Helpers;
using MambaApi.Core.Entities;
using MambaApi.Core.Repositories;
using MambaApi.Data.DataAccessLayer;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MambaApi.Business.Services.Implementations
{
    public class WorkerService : IWorkerService
    {
        private readonly IWorkerRepository _workerRepository;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _webHostEnvironment;
        private readonly IWorkerProfessionRepository _workerProfessionRepository;
        private readonly AppDbContext _context;

        public WorkerService(IWorkerRepository workerRepository,
                                IMapper mapper,
                                IWebHostEnvironment webHostEnvironment,
                                IWorkerProfessionRepository workerProfessionRepository,
                                AppDbContext context)
        {
            _workerRepository = workerRepository;
            _mapper = mapper;
            _webHostEnvironment = webHostEnvironment;
            _workerProfessionRepository = workerProfessionRepository;
            _context = context;
        }

        public async Task CreateAsync([FromForm] WorkerCreateDto workerCreateDto)
        {
            Worker worker = _mapper.Map<Worker>(workerCreateDto);
            bool check = false;

            if (workerCreateDto.ProfessionIds != null)
            {
                foreach (int professionId in workerCreateDto.ProfessionIds)
                {
                    if (!_context.WorkerProfessions.Any(profession => profession.Id == professionId))
                    {
                        check = true;
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

                        await _workerProfessionRepository.CreateAsync(workerProfession);
                    }
                }
            }
            else
            {
                throw new notFound("professionId is not found");

            }

            if (workerCreateDto.ImgFile != null)
            {
                if (workerCreateDto.ImgFile.ContentType != "image/png" && workerCreateDto.ImgFile.ContentType != "image/jpeg")
                {
                    throw new InvalidImageContentTypeOrSize("enter the correct image contenttype!");
                }

                if (workerCreateDto.ImgFile.Length > 1048576)
                {
                    throw new InvalidImageContentTypeOrSize("image size must be less than 1mb!");
                }
            }
            else
            {
                throw new InvalidImage("Image is required!");
            }

            string folder = "Uploads/workers-images";
            string newImgUrl = await Helper.GetFileName(_webHostEnvironment.WebRootPath, folder, workerCreateDto.ImgFile);


            worker.CreatedDate = DateTime.UtcNow.AddHours(4);
            worker.UpdatedDate = DateTime.UtcNow.AddHours(4);
            worker.DeletedDate = DateTime.UtcNow.AddHours(4);

            worker.ImgUrl = newImgUrl;
            worker.IsDeleted = false;

            await _workerRepository.CreateAsync(worker);
            await _workerRepository.CommitChanges();
        }

        public Task Delete(int id)
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<WorkerGetDto>> GetAllAsync(string? input, int? professionId, int? orderId)
        {
            IQueryable<Worker> workers = _workerRepository.Table.Include(worker => worker.WorkerProfessions).Where(worker => worker.IsDeleted == false).AsQueryable();
            if (workers is not null)
            {
                if (input is not null)
                {
                    workers = workers.Where(worker => worker.FullName.ToLower().Contains(input.ToLower()) || worker.Description.ToLower().Contains(input.ToLower()));
                }

                if (professionId is not null)
                {
                    workers = workers.Where(worker => worker.WorkerProfessions.Any(worker => worker.ProfessionId == professionId));
                }

                if (orderId is not null)
                {
                    switch (orderId)
                    {
                        case 1:
                            workers = workers.OrderByDescending(worker => worker.CreatedDate);
                            break;
                        case 2:
                            workers = workers.OrderBy(worker => worker.Salary);
                            break;
                        case 3:
                            workers = workers.OrderBy(worker => worker.FullName);
                            break;
                        default:
                            throw new notFound("enter the correct order value!");
                    }
                }
            }

            IEnumerable<WorkerGetDto> workerGetDtos = workers.Select(worker => new WorkerGetDto { Id = worker.Id, FullName = worker.FullName, Description = worker.Description, MediaUrl = worker.MediaUrl, Salary = worker.Salary });

            return workerGetDtos;
        }

        public async Task<WorkerGetDto> GetByIdAsync(int id)
        {

            Worker worker = await _workerRepository.GetByIdAsync(worker => worker.Id == id && worker.IsDeleted == false);

            if (worker == null) throw new notFound("worker couldn't be null!");

            WorkerGetDto workerGetDto = _mapper.Map<WorkerGetDto>(worker);

            return workerGetDto;
        }

        public IQueryable<Worker> GetWorkerTable()
        {
            var query = _workerRepository.Table.AsQueryable();

            return query;
        }

        public async Task ToggleDelete(int id)
        {
            string folder = "Uploads/workers-images";

            Worker worker = await _workerRepository.GetByIdAsync(worker => worker.Id == id);

            if (worker == null) throw new notFound("worker couldn't be null!");

            string fullPath = Path.Combine(_webHostEnvironment.WebRootPath, folder, worker.ImgUrl);

            if (System.IO.File.Exists(fullPath))
            {
                System.IO.File.Delete(fullPath);
            }

            worker.IsDeleted = !worker.IsDeleted;
            worker.DeletedDate = DateTime.UtcNow.AddHours(4);

            await _workerRepository.CommitChanges();
        }

        public async Task UpdateAsync([FromForm] WorkerUpdateDto workerUpdateDto)
        {
            Worker worker = await _workerRepository.GetByIdAsync(worker => worker.Id == workerUpdateDto.Id && worker.IsDeleted == false, "WorkerProfessions.Profession");

            if (worker == null) throw new notFound("worker couldn't be null!");

            worker.WorkerProfessions.RemoveAll(wp => !workerUpdateDto.ProfessionIds.Contains(wp.ProfessionId));

            foreach (var professionId in workerUpdateDto.ProfessionIds.Where(pId => !worker.WorkerProfessions.Any(wp => wp.ProfessionId == pId)))
            {
                WorkerProfession workerProfession = new WorkerProfession
                {
                    Worker = worker,
                    ProfessionId = professionId,
                };

                await _workerProfessionRepository.CreateAsync(workerProfession);
            }

            if (workerUpdateDto.ImgFile != null)
            {
                if (workerUpdateDto.ImgFile.ContentType != "image/png" && workerUpdateDto.ImgFile.ContentType != "image/jpeg")
                {
                    throw new InvalidImageContentTypeOrSize("enter the correct image contenttype!");
                }

                if (workerUpdateDto.ImgFile.Length > 1048576)
                {
                    throw new InvalidImageContentTypeOrSize("image size must be less than 1mb!");
                }

                string folder = "Uploads/workers-images";
                string newImgUrl = await Helper.GetFileName(_webHostEnvironment.WebRootPath, folder, workerUpdateDto.ImgFile);

                string oldImgPath = Path.Combine(_webHostEnvironment.WebRootPath, folder, worker.ImgUrl);

                if (System.IO.File.Exists(oldImgPath))
                {
                    System.IO.File.Delete(oldImgPath);
                }

                worker.ImgUrl = newImgUrl;

            }

            worker = _mapper.Map(workerUpdateDto, worker);
            worker.UpdatedDate = DateTime.UtcNow.AddHours(4);

            await _workerRepository.CommitChanges();
        }
    }
}
