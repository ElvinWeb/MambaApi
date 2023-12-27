using FluentValidation;

namespace MambaApi.Business.DTO.ProfessionDtos
{
    public class ProfessionCreateDto
    {
        public string Name { get; set; }


    }
    public class ProfessionCreateDtoValidator : AbstractValidator<ProfessionCreateDto>
    {
        public ProfessionCreateDtoValidator()
        {
            RuleFor(profession => profession.Name)
                .NotEmpty().WithMessage("Bos ola bilmez!")
                .NotNull().WithMessage("Null ola bilmez!")
                .MaximumLength(50).WithMessage("Max 50 ola biler!")
                .MinimumLength(5).WithMessage("Min 5 ola biler!");
        }
    }
}
