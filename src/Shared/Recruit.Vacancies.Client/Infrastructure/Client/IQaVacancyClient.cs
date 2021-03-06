using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Recruit.Vacancies.Client.Domain.Entities;
using Esfa.Recruit.Vacancies.Client.Infrastructure.QueryStore.Projections.QA;
using Esfa.Recruit.Vacancies.Client.Infrastructure.ReferenceData.Qualifications;

namespace Esfa.Recruit.Vacancies.Client.Infrastructure.Client
{
    public interface IQaVacancyClient
    {
        Task<QaDashboard> GetDashboardAsync();
        Task<Vacancy> GetVacancyAsync(long vacancyReference);
        Task<IApprenticeshipProgramme> GetApprenticeshipProgrammeAsync(string programmeId);
        Task ApproveVacancyReviewAsync(Guid reviewId, string manualQaComment, List<ManualQaFieldIndicator> manualQaFieldIndicators, List<Guid> automatedQaRuleOutcomeIds);
        Task<VacancyReview> GetVacancyReviewAsync(Guid reviewId);
        Task ReferVacancyReviewAsync(Guid reviewId, string manualQaComment, List<ManualQaFieldIndicator> manualQaFieldIndicators, List<Guid> automatedQaRuleOutcomeIds);
        Task ApproveReferredReviewAsync(Guid reviewId, string shortDescription, string vacancyDescription, string trainingDescription, string outcomeDescription, string thingsToConsider, string employerDescription);
        Task<Qualifications> GetCandidateQualificationsAsync();
        Task<List<VacancyReview>> GetSearchResultsAsync(string searchTerm);
        Task<int> GetApprovedCountAsync(string submittedByUserId);
        Task<List<VacancyReview>> GetVacancyReviewsInProgressAsync();
        Task<int> GetApprovedFirstTimeCountAsync(string submittedByUserId);
        Task AssignNextVacancyReviewAsync(VacancyUser user);
        Task AssignVacancyReviewAsync(VacancyUser user, Guid reviewId);
        Task<List<VacancyReview>> GetAssignedVacancyReviewsForUserAsync(string userId);
        bool VacancyReviewCanBeAssigned(VacancyReview review);
        bool VacancyReviewCanBeAssigned(ReviewStatus status, DateTime? reviewedDate);
        Task UnassignVacancyReviewAsync(Guid reviewId);
        Task<VacancyReview> GetCurrentReferredVacancyReviewAsync(long vacancyReference);
        Task<List<VacancyReview>> GetVacancyReviewHistoryAsync(long vacancyReference);
    }
}