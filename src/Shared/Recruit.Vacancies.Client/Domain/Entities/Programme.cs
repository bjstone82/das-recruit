﻿using System;
using System.Collections.Generic;
using System.Text;
using Esfa.Recruit.Vacancies.Client.Domain.Projections;

namespace Esfa.Recruit.Vacancies.Client.Domain.Entities
{
    public class Programme
    {
        public string Id { get; set; }

        public string Title { get; set; }

        public TrainingType? TrainingType { get; set; }

        public int? Level { get; set; }

        public string LevelName { get; set; }
    }
}