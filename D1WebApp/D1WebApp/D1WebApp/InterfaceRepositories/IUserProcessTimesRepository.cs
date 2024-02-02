using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace D1WebApp.InterfaceRepositories
{
    public interface IUserProcessTimesRepository : IDisposable
    {
        List<SchedulerConfigViewModels> GetSchedulerConfigByUserID(long UserID);
        List<ProcessTimesViewModel> GetProcessTimesList();
        bool InsertUserProcessTimes(UserProcessTimesViewModel UserProcessTimesView);
        UserProcessTimesViewModel GetUserProcessTimesByUserID(long UserID);


    }
}
