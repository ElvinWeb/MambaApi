﻿using FluentValidation;

namespace MambaApi.DTO.WorkerDtos
{
    public class WorkerCreateDto
    {
        public string FullName { get; set; }
        public string Description { get; set; }
        public string MediaUrl { get; set; }
        public List<int> ProfessionIds { get; set; }
        public IFormFile ImgFile { get; set; }
    }
    public class WorkerCreateDtoValidator : AbstractValidator<WorkerCreateDto>
    {
        public WorkerCreateDtoValidator()
        {
            RuleFor(worker => worker.FullName)
               .NotEmpty().WithMessage("Bos ola bilmez!")
               .NotNull().WithMessage("Null ola bilmez!")
               .MaximumLength(50).WithMessage("Max 50 ola biler!")
               .MinimumLength(5).WithMessage("Min 5 ola biler!");


            RuleFor(worker => worker.Description)
               .NotEmpty().WithMessage("Bos ola bilmez!")
               .NotNull().WithMessage("Null ola bilmez!")
               .MaximumLength(100).WithMessage("Max 100 ola biler!")
               .MinimumLength(5).WithMessage("Min 5 ola biler!");


            RuleFor(worker => worker.MediaUrl)
               .NotEmpty().WithMessage("Bos ola bilmez!")
               .NotNull().WithMessage("Null ola bilmez!")
               .MaximumLength(150).WithMessage("Max 100 ola biler!")
               .MinimumLength(10).WithMessage("Min 10 ola biler!");

            RuleFor(worker => worker.ImgFile)
             .NotEmpty().WithMessage("Bos ola bilmez!")
             .NotNull().WithMessage("Null ola bilmez!");
        }
    }
}
