﻿using System;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;
using Microsoft.Extensions.Logging;

namespace Esfa.Recruit.Shared.Web.ModelBinders
{
    public class TrimModelBinderProvider : IModelBinderProvider
    {
        private readonly ILoggerFactory _loggerFactory;

        public TrimModelBinderProvider(ILoggerFactory loggerFactory)
        {
            _loggerFactory = loggerFactory;
        }

        public IModelBinder GetBinder(ModelBinderProviderContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (!context.Metadata.IsComplexType && context.Metadata.ModelType == typeof(string))
            {
                return new TrimModelBinder(new SimpleTypeModelBinder(context.Metadata.ModelType, _loggerFactory));
            }
            return null;
        }
    }
}
