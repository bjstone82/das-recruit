﻿using System.Linq;
using Esfa.Recruit.Qa.Web.ModelBinders;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding.Binders;

namespace Esfa.Recruit.Qa.Web.Extensions
{
    public static class MvcOptionsExtensions
    {
        public static void AddTrimModelBinderProvider(this MvcOptions option)
        {
            var binderToFind = option.ModelBinderProviders
                .FirstOrDefault(x => x.GetType() == typeof(SimpleTypeModelBinderProvider));
            if (binderToFind == null)
            {
                return;
            }
            var index = option.ModelBinderProviders.IndexOf(binderToFind);
            option.ModelBinderProviders.Insert(index, new TrimModelBinderProvider());
        }
    }
}