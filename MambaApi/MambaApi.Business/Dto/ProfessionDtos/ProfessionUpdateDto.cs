using FluentValidation;

namespace MambaApi.Business.DTO.ProfessionDtos
{
    public class ProfessionUpdateDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
    public class ProfessionUpdateDtoValidator : AbstractValidator<ProfessionUpdateDto>
    {
        public ProfessionUpdateDtoValidator()
        {

            RuleFor(profession => profession.Name)
                .NotEmpty().WithMessage("Bos ola bilmez!")
                .NotNull().WithMessage("Null ola bilmez!")
                .MaximumLength(50).WithMessage("Max 50 ola biler!")
                .MinimumLength(10).WithMessage("Min 10 ola biler!");

            RuleFor(profession => profession.Id)
                 .NotEmpty().WithMessage("Bos ola bilmez!")
                 .NotNull().WithMessage("Null ola bilmez!")
                 .GreaterThanOrEqualTo(1).WithMessage("Id 0 ve ya menfi bir deyer ola bilmez!");
        }
    }
}
