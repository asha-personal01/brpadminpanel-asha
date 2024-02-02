using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.InterfaceRepositories;
using D1WebApp.Models;
using D1WebApp.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace D1WebApp.Repositories
{
    public class UserProcessTimesRepository : IUserProcessTimesRepository,IDisposable
    {
        private D1WebAppEntities context;
        public UserProcessTimesRepository(D1WebAppEntities context)
        {
            this.context = context;
        }
        public List<ProcessTimesViewModel> GetProcessTimesList()
        {
            return ProcessTimesViewModel.ConvertToViewModelList(context.ProcessTimes.ToList());
        }
        public bool InsertUserProcessTimes(UserProcessTimesViewModel UserProcessTimesView)
        {
            bool flag = false;
            try
            {
                UserProcessTime getrec = new UserProcessTime();
                getrec = UserProcessTimesViewModel.ConvertToModel(UserProcessTimesView);
                var getrecord = context.UserProcessTimes.Where(cp => cp.UserID == UserProcessTimesView.UserID).FirstOrDefault();
                if (getrecord != null && getrecord.UserID > 0)
                {
                    getrec.UserProcessTimeID = getrecord.UserProcessTimeID;                    
                    context.Entry(getrecord).CurrentValues.SetValues(getrec);
                    Save();
                }
                else
                {
                    context.UserProcessTimes.Add(getrec);
                    Save();
                }
                flag = true;
            }
            catch(Exception ed) { flag = false; }
            return flag;
        }
        public UserProcessTimesViewModel GetUserProcessTimesByUserID(long UserID)
        {
            UserProcessTimesViewModel UserProcessTimesView = new UserProcessTimesViewModel();
            var getrec = context.UserProcessTimes.Where(cp => cp.UserID == UserID).FirstOrDefault();
            if(getrec!=null && getrec.UserID > 0)
            {
                UserProcessTimesView = UserProcessTimesViewModel.ConvertToViewModel(getrec);
            }
            return UserProcessTimesView;

        }
        public List<SchedulerConfigViewModels> GetSchedulerConfigByUserID(long UserID)
        {
            List<SchedulerConfigViewModels> UserProcessTimesView = new List<SchedulerConfigViewModels>();
            UserProcessTimesView = (from sch in context.SchedulerConfigs.AsNoTracking()
                                    where sch.UserId == UserID
                                    select new SchedulerConfigViewModels {
                                        SchedulerConfigId=sch.SchedulerConfigId,
                                        UserId=sch.UserId,
                                        SchedulerName=sch.SchedulerName,
                                        SchedulerTables=sch.SchedulerTables,
                                        SchedulerTime=sch.SchedulerTime,
                                        Active = sch.Active
                                    }).ToList();
            return UserProcessTimesView;

        }

        public void Save()
        {
            context.SaveChanges();
        }

        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}