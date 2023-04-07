using FluentValidation;

namespace SApInterface.API.Validators
{
    public class AddSectionRequestValidator : AbstractValidator<Model.DTO.AddSectionRequest>
    {
        public AddSectionRequestValidator()
        {
            RuleFor(x => x.sectionCode).NotEmpty();
            RuleFor(x => x.sectionName).NotEmpty();
            //RuleFor(x => x.Area).GreaterThan(0);
            //RuleFor(x => x.Population).GreaterThanOrEqualTo(0);
        }
    }
}
