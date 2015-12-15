namespace MultiDbSupportWithConventions.Features.Users.AddUser
{
    using FluentValidation;

    public class AddUserValidator : AbstractValidator<AddUserCommand>
    {
        public AddUserValidator()
        {
            this.RuleFor(x => x.Email).NotEmpty();
            this.RuleFor(x => x.FirstName).NotEmpty();
            this.RuleFor(x => x.LastName).NotEmpty();
        }
    }
}