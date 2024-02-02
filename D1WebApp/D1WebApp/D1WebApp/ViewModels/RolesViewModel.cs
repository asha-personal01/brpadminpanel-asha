using D1WebApp.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace D1WebApp.BusinessLogicLayer.ViewModels
{
    public class RoleViewModel
    {
        public long RoleID { get; set; }        
        public string RoleName { get; set; }
        public bool IsActive { get; set; }        
        public string Created { get; set; }
        public System.DateTime CreatedOn { get; set; }
        public long CreatedBy { get; set; }
        public string Modified { get; set; }
        public Nullable<System.DateTime> ModifiedOn { get; set; }
        public Nullable<long> ModifiedBy { get; set; }
        public RoleViewModel()
        { }

        public RoleViewModel(Role Roles)
        {
            this.RoleID = Roles.RoleID;
            this.RoleName = Roles.RoleName;
            this.IsActive = Roles.IsActive;
            
        }

        /// <summary>
        /// Convert Model to ViewModel
        /// </summary>
        /// <param name="Roles">Role list from model</param>
        /// <returns>Role list in ViewModel</returns>
        public static List<RoleViewModel> ConvertModelToViewModel(List<Role> Roles)
        {
            List<RoleViewModel> RoleList = new List<RoleViewModel>();
            foreach (Role list in Roles)
            {
                RoleList.Add(new RoleViewModel(list));
            }
            return RoleList;
        }

        /// <summary>
        /// Convert ViewModel to Model
        /// </summary>
        /// <param name="Role">Viewmodel of Role</param>
        /// <returns>Model of Role</returns>
        public static Role ConvertViewModelToModel(RoleViewModel Role)
        {
            Role Roles = new Role();
            Roles.RoleName = Role.RoleName.ToUpper();
            Roles.IsActive = Role.IsActive;            
            return Roles;
        }

        /// <summary>
        /// Update Roles
        /// </summary>
        /// <param name="Role">Object of Role ViewModel</param>
        /// <param name="Roles">Object of Role Model</param>
        /// <returns>Role Model Updated</returns>
        public static Role UpdateRole(RoleViewModel Role, Role Roles)
        {
            Roles.RoleID = Role.RoleID;
            Roles.RoleName = Role.RoleName.ToUpper();
            Roles.IsActive = Role.IsActive;
            
            return Roles;
        }
    }

    public class UserAccessTab
    {
        public string RoleName { get; set; }
        public string TabAccess { get; set; }
        public long? UserParent { get; set; }
        public string UserParentMemberRefNo{ get; set; }
        public string UserParentRoleName { get; set; }

    }
}