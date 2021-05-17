using FluentValidation;
using StockManagement.Api.ViewModels.Input;

namespace StockManagement.Api.Validators
{
    public class StockInputViewModelValidator : AbstractValidator<StockInputViewModel>
    {
        public StockInputViewModelValidator()
        {
            RuleFor(x => x.Amount).GreaterThan(0);
        }
    }
}