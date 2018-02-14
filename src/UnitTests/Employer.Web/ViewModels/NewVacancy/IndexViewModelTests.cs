﻿using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using FluentAssertions;
using Xunit;
using EmployerWeb = Esfa.Recruit.Employer.Web;

namespace Esfa.Recruit.Employer.UnitTests.Employer.Web.ViewModels.NewVacancy
{
    public class IndexEditModelTests
    {
        [Fact]
        public void ShouldErrorIfTitleIsNotSpecified()
        {
            var vm = new EmployerWeb.ViewModels.NewVacancy.IndexViewModel
            {
                Title = null
            };

            var context = new ValidationContext(vm, null, null);
            var result = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(vm, context, result, true);
            
            isValid.Should().BeFalse();
            result.Should().HaveCount(1);
            result.Single(r => r.MemberNames.Single() == "Title").ErrorMessage.Should().Be("The Title field is required.");
        }

        [Fact]
        public void ShouldBeValidIfTitleIsSpecified()
        {
            var vm = new EmployerWeb.ViewModels.NewVacancy.IndexViewModel
            {
                Title = "some text"
            };

            var context = new ValidationContext(vm, null, null);
            var result = new List<ValidationResult>();

            // Act
            var isValid = Validator.TryValidateObject(vm, context, result, false);

            isValid.Should().BeTrue();
        }
    }
}