﻿namespace Esfa.Recruit.Employer.Web.ViewModels.Validations
{
    using Esfa.Recruit.Employer.Web.Extensions;    
    using System.ComponentModel.DataAnnotations;

    public class TypeOfMoneyAttribute : ValidationAttribute
    {
        public override bool IsValid(object value)
        {
            if (value == null)
            {
                return true;
            }

            return (((string)value).AsMoney() != null);            
        }
    }
}