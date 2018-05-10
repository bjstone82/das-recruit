﻿namespace Esfa.Recruit.Qa.Web.ModelBinders
{
    using Microsoft.AspNetCore.Mvc.ModelBinding;
    using System;
    using System.Threading.Tasks;
    public class TrimModelBinder : IModelBinder
    {
        private readonly IModelBinder _fallbackBinder;

        public TrimModelBinder(IModelBinder fallbackBinder)
        {
            _fallbackBinder = fallbackBinder ?? throw new ArgumentNullException(nameof(fallbackBinder));
        }
        public Task BindModelAsync(ModelBindingContext bindingContext)
        {
            if (bindingContext == null)
            {
                throw new ArgumentNullException(nameof(bindingContext));
            }
            var valueProviderResult = bindingContext.ValueProvider.GetValue(bindingContext.ModelName);

            if (valueProviderResult.FirstValue is string str &&
                !string.IsNullOrEmpty(str))
            {
                bindingContext.Result = ModelBindingResult.Success(str.Trim());
                return Task.CompletedTask;
            }
            return _fallbackBinder.BindModelAsync(bindingContext);
        }
    }
}