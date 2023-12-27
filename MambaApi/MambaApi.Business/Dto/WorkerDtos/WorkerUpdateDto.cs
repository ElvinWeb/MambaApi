using FluentValidation;
using Microsoft.AspNetCore.Http;

namespace MambaApi.Business.DTO.WorkerDtos
{
    public class WorkerUpdateDto
    {
        public int Id { get; set; }
        public string FullName { get; set; }
        public string Description { get; set; }
        public double Salary { get; set; }
        public string MediaUrl { get; set; }
        public List<int> ProfessionIds { get; set; }
        public IFormFile ImgFile { get; set; }
    }

    public class WorkerUpdateDtoValidator : AbstractValidator<WorkerUpdateDto>
    {
        public WorkerUpdateDtoValidator()
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

            RuleFor(worker => worker.Salary)
               .NotEmpty().WithMessage("Bos ola bilmez!")
               .NotNull().WithMessage("Null ola bilmez!")
               .GreaterThan(0).WithMessage("maas menifi ola bilmez!");

            RuleFor(worker => worker.ImgFile)
             .NotEmpty().WithMessage("Bos ola bilmez!")
             .NotNull().WithMessage("Null ola bilmez!");

            RuleFor(worker => worker.Id)
            .NotEmpty().WithMessage("Bos ola bilmez!")
            .NotNull().WithMessage("Null ola bilmez!")
            .GreaterThanOrEqualTo(1).WithMessage("Id 0 ve ya menfi bir deyer ola bilmez!");
        }
    }
}
