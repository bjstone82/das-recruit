﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Esfa.Recruit.Vacancies.Client.Domain.Entities;
using Esfa.Recruit.Vacancies.Client.Domain.Repositories;
using Esfa.Recruit.Vacancies.Client.Infrastructure.Mongo;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MongoDB.Driver;
using Polly;

namespace Esfa.Recruit.Vacancies.Client.Infrastructure.Repositories
{
    internal sealed class MongoDbVacancyReviewRepository : MongoDbCollectionBase, IVacancyReviewRepository, IVacancyReviewQuery
    {

        private const string Status = "status";


        public MongoDbVacancyReviewRepository(ILoggerFactory loggerFactory, IOptions<MongoDbConnectionDetails> details) 
            : base(loggerFactory, MongoDbNames.RecruitDb, MongoDbCollectionNames.VacancyReviews, details)
        {
        }

        public async Task<VacancyReview> GetLatestReviewByReferenceAsync(long vacancyReference)
        {
            var filterBuilder = Builders<VacancyReview>.Filter;

            var filter = !filterBuilder.Eq(r => r.Status, ReviewStatus.New)
                         & filterBuilder.Eq(r => r.VacancyReference, vacancyReference);
            var results = await GetVacancyReviewsAsync(filter);
            return results.OrderByDescending(r => r.CreatedDate).FirstOrDefault();
        }

        public Task<List<VacancyReview>> GetVacancyReviewsInProgressAsync(DateTime reviewExpiration)
        {
            var filterBuilder = Builders<VacancyReview>.Filter;
            var filter = filterBuilder.Eq(r => r.Status, ReviewStatus.UnderReview)
                & filterBuilder.Gt(r => r.ReviewedDate, reviewExpiration);
            return GetVacancyReviewsAsync(filter);
        }

        private async Task<List<VacancyReview>> GetVacancyReviewsAsync(FilterDefinition<VacancyReview> filter)
        {
            var collection = GetCollection<VacancyReview>();

            var result = await RetryPolicy
                .ExecuteAsync(
                    context => collection
                        .Find(filter)
                        .ToListAsync(),
                    new Context(nameof(GetLatestReviewByReferenceAsync)));

            return result;
        }

        public Task CreateAsync(VacancyReview vacancy)
        {
            var collection = GetCollection<VacancyReview>();
            return RetryPolicy.ExecuteAsync(context => collection.InsertOneAsync(vacancy), new Context(nameof(CreateAsync)));
        }

        public async Task<VacancyReview> GetAsync(Guid reviewId)
        {
            var filter = Builders<VacancyReview>.Filter.Eq(r => r.Id, reviewId);

            var collection = GetCollection<VacancyReview>();
            var result = await RetryPolicy.ExecuteAsync(context => collection.FindAsync(filter), new Context(nameof(GetAsync)));
            return result.SingleOrDefault();
        }

        public async Task<List<VacancyReview>> GetByStatusAsync(ReviewStatus status)
        {
            var filter = Builders<VacancyReview>.Filter.Eq(r => r.Status, status);

            var collection = GetCollection<VacancyReview>();
            var result = await RetryPolicy.ExecuteAsync(context => collection
                .Find(filter)
                .ToListAsync(), new Context(nameof(GetByStatusAsync)));

            return result;
        }

        public async Task<List<T>> GetActiveAsync<T>()
        {
            var filterBuilder = Builders<T>.Filter;

            var filter = filterBuilder.Eq(Status, ReviewStatus.PendingReview.ToString())
                         | filterBuilder.Eq(Status, ReviewStatus.UnderReview.ToString());

            var collection = GetCollection<T>();

            var result = await RetryPolicy.ExecuteAsync(context => collection.Find(filter)
                    .Project<T>(GetProjection<T>())
                    .ToListAsync()
            , new Context(nameof(GetActiveAsync)));

            return result;
        }

        public Task UpdateAsync(VacancyReview review)
        {
            var filterBuilder = Builders<VacancyReview>.Filter;
            var filter = filterBuilder.Eq(v => v.Id, review.Id) & filterBuilder.Eq(v => v.VacancyReference, review.VacancyReference);
            var collection = GetCollection<VacancyReview>();
           
            return RetryPolicy.ExecuteAsync(context => collection.ReplaceOneAsync(filter, review), new Context(nameof(UpdateAsync)));
        }

        public Task<List<VacancyReview>> GetForVacancyAsync(long vacancyReference)
        {
            var filter = Builders<VacancyReview>.Filter.Eq(r => r.VacancyReference, vacancyReference);

            var collection = GetCollection<VacancyReview>();
            return RetryPolicy.ExecuteAsync(context => collection
                .Find(filter)
                .ToListAsync(), new Context(nameof(GetForVacancyAsync)));
        }

        public async Task<int> GetApprovedFirstTimeCountAsync(string submittedByUserId)
        {
            var filterBuilder = Builders<VacancyReview>.Filter;
            var filter = filterBuilder.Eq(r => r.SubmittedByUser.UserId, submittedByUserId) &
                         filterBuilder.Eq(r => r.Status, ReviewStatus.Closed) &
                         filterBuilder.Eq(r => r.ManualOutcome, ManualQaOutcome.Approved) &
                         filterBuilder.Eq(r => r.SubmissionCount, 1);

            var collection = GetCollection<VacancyReview>();
            var count = await RetryPolicy.ExecuteAsync(context => collection
                .CountDocumentsAsync(filter), 
                new Context(nameof(GetApprovedFirstTimeCountAsync)));

            return (int) count;
        }

        public async Task<int> GetApprovedCountAsync(string submittedByUserId)
        {
            var filterBuilder = Builders<VacancyReview>.Filter;
            var filter = filterBuilder.Eq(r => r.SubmittedByUser.UserId, submittedByUserId) &
                         filterBuilder.Eq(r => r.Status, ReviewStatus.Closed) &
                         filterBuilder.Eq(r => r.ManualOutcome, ManualQaOutcome.Approved);

            var collection = GetCollection<VacancyReview>();
            var count = await RetryPolicy.ExecuteAsync(context => collection
                    .CountDocumentsAsync(filter),
                new Context(nameof(GetApprovedCountAsync)));

            return (int)count;
        }

        public async Task<List<VacancyReview>> GetAssignedForUserAsync(string userId, DateTime assignationExpiryDateTime)
        {
            var filterBuilder = Builders<VacancyReview>.Filter;

            var filter = filterBuilder.Eq(r => r.Status, ReviewStatus.UnderReview)
                         & filterBuilder.Eq(r => r.ReviewedByUser.UserId, userId)
                         & filterBuilder.Gt(r => r.ReviewedDate, assignationExpiryDateTime);

            var collection = GetCollection<VacancyReview>();
            var result = await RetryPolicy.ExecuteAsync(context => collection
                .Find(filter)
                .ToListAsync(), new Context(nameof(GetAssignedForUserAsync)));

            return result.OrderByDescending(x => x.ReviewedDate).ToList();
        }

        public async Task<VacancyReview> GetCurrentReferredVacancyReviewAsync(long vacancyReference)
        {
            var vacancyReviews = await GetForVacancyAsync(vacancyReference);

            var vacancyReview = vacancyReviews.Where(r => r.Status == ReviewStatus.Closed &&
                                                          r.ManualOutcome == ManualQaOutcome.Referred)
                .OrderByDescending(r => r.ClosedDate)
                .FirstOrDefault();

            return vacancyReview;
        }
    }
}
