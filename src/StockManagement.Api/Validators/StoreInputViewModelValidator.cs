using FluentValidation;
using StockManagement.Api.ViewModels.Input;

namespace StockManagement.Api.Validators
{
    public class StoreInputViewModelValidator : AbstractValidator<StoreInputViewModel>
    {
        public StoreInputViewModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.Address).NotEmpty();
        }
    }
}