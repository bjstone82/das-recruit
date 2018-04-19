﻿using System.Threading.Tasks;
using Esfa.Recruit.Employer.Web.Configuration;
using Esfa.Recruit.Employer.Web.Extensions;
using Esfa.Recruit.Employer.Web.Mappings;
using Esfa.Recruit.Employer.Web.Models;
using Esfa.Recruit.Employer.Web.ViewModels;
using Esfa.Recruit.Vacancies.Client.Domain.Entities;
using Esfa.Recruit.Vacancies.Client.Domain.Exceptions;
using Esfa.Recruit.Vacancies.Client.Domain.Services;

namespace Esfa.Recruit.Employer.Web.Orchestrators
{
    public class VacancyManageOrchestrator
    {
        private readonly DisplayVacancyViewModelMapper _vacancyDisplayMapper;
        private readonly ITimeProvider _timeProvider;

        public VacancyManageOrchestrator(DisplayVacancyViewModelMapper vacancyDisplayMapper, ITimeProvider timeProvider)
        {
            _vacancyDisplayMapper = vacancyDisplayMapper;
            _timeProvider = timeProvider;
        }

        public async Task<ManageVacancy> GetVacancyDisplayViewModelAsync(Vacancy vacancy)
        {
            if (vacancy.ClosingDate.HasValue && vacancy.ClosingDate.Value <= _timeProvider.Now)
            {
                return await GetClosedVacancyViewModel(vacancy);
            }

            switch (vacancy.Status)
            {
                case VacancyStatus.Submitted:
                    var submittedViewModel = new SubmittedVacancyViewModel();
                    await _vacancyDisplayMapper.MapFromVacancyAsync(submittedViewModel, vacancy);
                    submittedViewModel.SubmittedDate = vacancy.SubmittedDate.Value.AsDisplayDate();
                    return new ManageVacancy
                    {
                        ViewModel = submittedViewModel,
                        ViewName = ViewNames.ManageSubmittedVacancyView
                    };
                case VacancyStatus.Live:
                    var liveViewModel = new LiveVacancyViewModel();
                    await _vacancyDisplayMapper.MapFromVacancyAsync(liveViewModel, vacancy);
                    return new ManageVacancy
                    {
                        ViewModel = liveViewModel,
                        ViewName = ViewNames.ManageLiveVacancyView
                    };
                case VacancyStatus.Closed:
                    return await GetClosedVacancyViewModel(vacancy);
                case VacancyStatus.Referred:
                    var referredViewModel = new ReferredVacancyViewModel();
                    await _vacancyDisplayMapper.MapFromVacancyAsync(referredViewModel, vacancy);
                    return new ManageVacancy
                    {
                        ViewModel = referredViewModel,
                        ViewName = ViewNames.ManageReferredVacancyView
                    };
                default:
                    throw new InvalidStateException(string.Format(ErrorMessages.VacancyCannotBeViewed, vacancy.Title));
            }
        }

        private async Task<ManageVacancy> GetClosedVacancyViewModel(Vacancy vacancy)
        {
            var closedViewModel = new ClosedVacancyViewModel();
            await _vacancyDisplayMapper.MapFromVacancyAsync(closedViewModel, vacancy);
            return new ManageVacancy
            {
                ViewModel = closedViewModel,
                ViewName = ViewNames.ManageClosedVacancyView
            };
        }
    }
}
