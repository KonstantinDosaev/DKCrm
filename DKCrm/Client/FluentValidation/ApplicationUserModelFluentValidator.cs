using DKCrm.Shared.Models;
using FluentValidation;


namespace DKCrm.Client.FluentValidation
{
    public class ApplicationUserModelFluentValidator : AbstractValidator<ApplicationUser>
    {
        public ApplicationUserModelFluentValidator()
        {
            RuleFor(x => x.UserName)
                .NotEmpty()
                .Length(1, 100);

            RuleFor(x => x.Email)
                .Cascade(CascadeMode.Stop)
                .NotEmpty()
                .EmailAddress()
                .MustAsync(async (value, cancellationToken) => await IsUniqueAsync(value));

            RuleFor(x => x.FirstName)
                .Length(1, 100);
            RuleFor(x => x.LastName)
                .Length(1, 100);

            RuleFor(x => x.Address).SetValidator(new AddressModelFluentValidator());
            RuleFor(x => x.AdditionalAddress).SetValidator(new AddressModelFluentValidator());
        }

        private async Task<bool> IsUniqueAsync(string email)
        {
            // Simulates a long running http call
            await Task.Delay(2000);
            return email.ToLower() != "test@test.com";
        }

        public Func<object, string, Task<IEnumerable<string>>> ValidateValue => async (model, propertyName) =>
        {
            var result = await ValidateAsync(ValidationContext<ApplicationUser>.CreateWithOptions((ApplicationUser)model, x => x.IncludeProperties(propertyName)));
            if (result.IsValid)
                return Array.Empty<string>();
            return result.Errors.Select(e => e.ErrorMessage);
        };
    }
}
