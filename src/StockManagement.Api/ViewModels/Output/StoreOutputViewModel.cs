using System;
using StockManagement.Api.ViewModels.Input;

namespace StockManagement.Api.ViewModels.Output
{
    public class StoreOutputViewModel : StoreInputViewModel
    {
        public Guid Id { get; set; }
    }
}