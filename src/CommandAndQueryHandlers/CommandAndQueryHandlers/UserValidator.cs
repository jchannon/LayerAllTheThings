using FluentValidation;

namespace QueryHandler
{
    public class UserValidator : AbstractValidator<User>
    {
        public UserValidator()
        {
            this.RuleFor(x => x.EmailAddress).NotEmpty();
            this.RuleFor(x => x.FirstName).NotEmpty();
            this.RuleFor(x => x.LastName).NotEmpty();
        }
    }
}
