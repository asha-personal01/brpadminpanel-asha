using D1WebApp.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using D1WebApp.Models;

namespace D1WebApp.ViewModels
{
    public class ProcessTimesViewModel
    {
        public int ProcessTimeID { get; set; }
        public string ProcessTimeName { get; set; }

        public static ProcessTimesViewModel ConvertToViewModel(ProcessTime NProcessTimes)
        {
            ProcessTimesViewModel ProcessTimesView = new ProcessTimesViewModel();
            ProcessTimesView.ProcessTimeID = NProcessTimes.ProcessTimeID;
            ProcessTimesView.ProcessTimeName = NProcessTimes.ProcessTimeName;
            return ProcessTimesView;
        }
        public static List<ProcessTimesViewModel> ConvertToViewModelList(List<ProcessTime> ProcessTimeList)
        {
            List<ProcessTimesViewModel> ProcessTimesViewList = new List<ProcessTimesViewModel>();
            foreach (var item in ProcessTimeList)
            {
                ProcessTimesViewList.Add(ProcessTimesViewModel.ConvertToViewModel(item));
            }
            return ProcessTimesViewList;
        }

    }
    public class UserProcessTimesViewModel
    {
        public string MemRefNo { get; set; }
        public long UserProcessTimeID { get; set; }
        public long UserID { get; set; }
        public int ProcessTimeID { get; set; }
        public string ProcessTimeName { get; set; }
        public static UserProcessTimesViewModel ConvertToViewModel(UserProcessTime UserProcessT)
        {
            UserProcessTimesViewModel UserProcessTimesView = new UserProcessTimesViewModel();
            UserProcessTimesView.ProcessTimeID = UserProcessT.ProcessTimeID;
            UserProcessTimesView.UserID = UserProcessT.UserID;
            UserProcessTimesView.UserProcessTimeID = UserProcessT.UserProcessTimeID;
            UserProcessTimesView.ProcessTimeName = UserProcessT.ProcessTime.ProcessTimeName;
            return UserProcessTimesView;
        }
        public static UserProcessTime ConvertToModel(UserProcessTimesViewModel UserProcessT)
        {
            UserProcessTime UserProcessTimesView = new UserProcessTime();
            UserProcessTimesView.ProcessTimeID = UserProcessT.ProcessTimeID;
            UserProcessTimesView.UserID = UserProcessT.UserID;
            if (UserProcessT.UserProcessTimeID != null && UserProcessT.UserProcessTimeID > 0)
            {
                UserProcessTimesView.UserProcessTimeID = UserProcessT.UserProcessTimeID;
            }
            return UserProcessTimesView;
        }
        public static List<UserProcessTimesViewModel> ConvertToViewModelList(List<UserProcessTime> UserProcessTimeList)
        {
            List<UserProcessTimesViewModel> UserProcessTimesViewList = new List<UserProcessTimesViewModel>();
            foreach (var item in UserProcessTimeList)
            {
                UserProcessTimesViewList.Add(UserProcessTimesViewModel.ConvertToViewModel(item));
            }
            return UserProcessTimesViewList;
        }
        public static List<UserProcessTime> ConvertToModelList(List<UserProcessTimesViewModel> UserProcessTimeList)
        {
            List<UserProcessTime> UserProcessTimesViewList = new List<UserProcessTime>();
            foreach (var item in UserProcessTimeList)
            {
                UserProcessTimesViewList.Add(UserProcessTimesViewModel.ConvertToModel(item));
            }
            return UserProcessTimesViewList;

        }
    }


    public class ProjectTypeViewModel
    {
        public int ProjectTypeID { get; set; }
        public string ProjectTypeName { get; set; }
        public static ProjectTypeViewModel ConvertToViewModel(ProjectType ptypes)
        {
            ProjectTypeViewModel ProjectTypeView = new ProjectTypeViewModel();
            ProjectTypeView.ProjectTypeID = ptypes.ProjectID;
            ProjectTypeView.ProjectTypeName = ptypes.ProjectType1;
            return ProjectTypeView;
        }
        public static List<ProjectTypeViewModel> ConvertToViewList(List<ProjectType> ptypes)
        {
            List<ProjectTypeViewModel> ConvertToView = new List<ProjectTypeViewModel>();
            foreach (var item in ptypes)
            {
                ConvertToView.Add(ProjectTypeViewModel.ConvertToViewModel(item));
            }
            return ConvertToView;
        }
    }

    public class SchedulerConfigViewModel
    {
        public string MemRefNo { get; set; }
        public string SchedulerName { get; set; }
        public string SchedulerTime { get; set; }
        public string SchedulerTables { get; set; }
        public long UserId { get; set; }
        public bool Active { get; set; }

        public static SchedulerConfigViewModel ConvertToViewModel(SchedulerConfig schedulerConfig)
        {
            SchedulerConfigViewModel schedulerConfigViewModel = new SchedulerConfigViewModel();
            schedulerConfigViewModel.SchedulerName = schedulerConfig.SchedulerName;
            schedulerConfigViewModel.UserId = schedulerConfig.UserId.Value;
            schedulerConfigViewModel.SchedulerTime = schedulerConfig.SchedulerTime;
            schedulerConfigViewModel.SchedulerTables = schedulerConfig.SchedulerTables;
            return schedulerConfigViewModel;
        }
        public static SchedulerConfig ConvertToModel(SchedulerConfigViewModel schedulerConfigViewModel)
        {
            SchedulerConfig schedulerConfig = new SchedulerConfig();
            schedulerConfig.SchedulerName = schedulerConfigViewModel.SchedulerName;
            schedulerConfig.UserId = schedulerConfigViewModel.UserId;
            schedulerConfig.SchedulerTime = schedulerConfigViewModel.SchedulerTime;
            schedulerConfig.SchedulerTables = schedulerConfigViewModel.SchedulerTables;
            schedulerConfig.Active = schedulerConfigViewModel.Active;
            return schedulerConfig;
        }
    }
}