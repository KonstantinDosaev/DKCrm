using DKCrm.Shared.Models;
using FluentValidation;

namespace DKCrm.Client.FluentValidation
{
    public class AddressModelFluentValidator : AbstractValidator<Address>
    {
        public AddressModelFluentValidator()
        {
            RuleFor(x => x.Country)
                .NotEmpty()
                .Length(1, 100);
            RuleFor(x => x.City)
                .NotEmpty()
                .Length(1, 100);
            RuleFor(x => x.Street)
                .NotEmpty()
                .Length(1, 100);
            RuleFor(x => x.Home)
                .NotEmpty()
                .Length(1, 30);
            RuleFor(x => x.Placement)
                .NotEmpty()
                .Length(1, 100);
            RuleFor(x => x.PostalCode)
                .NotEmpty()
                .Length(1, 30);

        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<Address>.CreateWithOptions((Address)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
