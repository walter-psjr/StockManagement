using FluentValidation;
using StockManagement.Api.ViewModels.Input;

namespace StockManagement.Api.Validators
{
    public class ProductInputViewModelValidator : AbstractValidator<ProductInputViewModel>
    {
        public ProductInputViewModelValidator()
        {
            RuleFor(x => x.Name).NotEmpty();
            RuleFor(x => x.CostPrice).GreaterThan(0);
        }
    }
}