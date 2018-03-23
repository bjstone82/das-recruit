﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Recruit.Employer.Web.Configuration;
using Esfa.Recruit.Employer.Web.ViewModels.Part2.Skills;
using Microsoft.Extensions.Options;

namespace Esfa.Recruit.Employer.Web.Services
{
    public class SkillsService : ISkillsService
    {

        private readonly SkillsConfiguration _skillsConfig;

        public SkillsService(IOptions<SkillsConfiguration> skillsConfigOptions)
        {
            _skillsConfig = skillsConfigOptions.Value;
        }

        public List<SkillViewModel> GetColumn1ViewModel(List<string> selected)
        {
            return _skillsConfig.Column1Skills.Select(c => new SkillViewModel
            {
                Name = c,
                Selected = selected != null && selected.Any(s => s == c)
            }).ToList();
        }

        public List<SkillViewModel> GetColumn2ViewModel(List<string> selected)
        {
            return _skillsConfig.Column2Skills.Select(c => new SkillViewModel
            {
                Name = c,
                Selected = selected != null && selected.Any(s => s == c)
            }).ToList();
        }

        public List<string> GetCustomSkills(List<string> selected)
        {
            return selected.Except(_skillsConfig.Column1Skills).Except(_skillsConfig.Column2Skills).ToList();
        }

        public string GetSkillsAsText(List<string> selected)
        {
            var orderedSkills = _skillsConfig.Column1Skills
                .Union(_skillsConfig.Column2Skills)
                .Union(selected)
                .ToList();
            
            return string.Join(", ", orderedSkills.Where(selected.Contains));
        }

    }
}
