using System;
using StockManagement.Api.ViewModels.Input;

namespace StockManagement.Api.ViewModels.Output
{
    public class ProductOutputViewModel : ProductInputViewModel
    {
        public Guid Id { get; set; }
    }
}