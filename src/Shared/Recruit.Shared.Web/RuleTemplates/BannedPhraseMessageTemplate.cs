﻿using Esfa.Recruit.Vacancies.Client.Domain.Entities;

namespace Esfa.Recruit.Shared.Web.RuleTemplates
{
    public static class BannedPhraseMessageTemplate
    {
        public static string ToText(BannedPhrasesData msgData, string fieldName)
        {
            return msgData.Occurrences > 1 
                ? $"{fieldName} contains the phrase {msgData.BannedPhrase} {msgData.Occurrences} times"
                : $"{fieldName} contains the phrase {msgData.BannedPhrase}";
        }
    }
}