using FluentValidation;

namespace Godwit.HandleEmailConfirmedEvent.Model.Validators {
    public class HasuraEventValidator : AbstractValidator<HasuraEvent> {
        public HasuraEventValidator() {
            RuleFor(x => x.Table.Name).Must(p => p == "users");
            RuleFor(x => x.Event.Operation).Must(p => p == "UPDATE");
            RuleFor(x => x.Event.Data.NewValue.IsConfirmed).Must(p => p);
            RuleFor(x => x.Event.Data.NewValue.Email).NotEmpty();
            RuleFor(x => x.Event.Data.NewValue.UserName).NotEmpty();
        }
    }
}