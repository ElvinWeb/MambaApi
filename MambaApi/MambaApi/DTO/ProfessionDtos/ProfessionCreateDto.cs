using FluentValidation;

namespace MambaApi.DTO.ProfessionDtos
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
                .MinimumLength(10).WithMessage("Min 10 ola biler!");
        }
    }
}
