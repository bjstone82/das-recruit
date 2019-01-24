using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Esfa.Recruit.Provider.Web.RouteModel;
using Esfa.Recruit.Provider.Web.ViewModels.Part1.Employer;
using Esfa.Recruit.Shared.Web.ViewModels;

namespace Provider.Web.Orchestrators.Part1
{
    public class EmployerOrchestrator
    {        
        public Task<EmployersViewModel> GetEmployersViewModelAsync(VacancyRouteModel vacancyRouteModel)
        {
            return Task.FromResult(
                new EmployersViewModel 
                {
                    PageInfo = new PartOnePageInfoViewModel(),
                    Employers = new List<EmployerViewModel>
                    {
                        new EmployerViewModel { Id = Guid.NewGuid().ToString(), Name = "Employer 1" },
                        new EmployerViewModel { Id = Guid.NewGuid().ToString(), Name = "Employer 2" }
                    }
                }
            );
        }
    }
}