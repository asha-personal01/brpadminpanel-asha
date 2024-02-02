﻿//------------------------------------------------------------------------------
// <auto-generated>
//     This code was generated from a template.
//
//     Manual changes to this file may cause unexpected behavior in your application.
//     Manual changes to this file will be overwritten if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

namespace D1WebApp.Models
{
    using System;
    using System.Data.Entity;
    using System.Data.Entity.Infrastructure;
    
    public partial class D1WebAppEntities : DbContext
    {
        public D1WebAppEntities()
            : base("name=D1WebAppEntities")
        {
        }
    
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            throw new UnintentionalCodeFirstException();
        }
    
        public virtual DbSet<Configuration> Configurations { get; set; }
        public virtual DbSet<ErrorLog> ErrorLogs { get; set; }
        public virtual DbSet<Notification> Notifications { get; set; }
        public virtual DbSet<ProcessTime> ProcessTimes { get; set; }
        public virtual DbSet<ProjectType> ProjectTypes { get; set; }
        public virtual DbSet<AuditTrailMaster> AuditTrailMasters { get; set; }
        public virtual DbSet<AuditTrail> AuditTrails { get; set; }
        public virtual DbSet<SchedulerConfig> SchedulerConfigs { get; set; }
        public virtual DbSet<MailBox> MailBoxes { get; set; }
        public virtual DbSet<MailQueue> MailQueues { get; set; }
        public virtual DbSet<MailTemplate> MailTemplates { get; set; }
        public virtual DbSet<UserMailBox> UserMailBoxes { get; set; }
        public virtual DbSet<RightGroup> RightGroups { get; set; }
        public virtual DbSet<RightRoleMapping> RightRoleMappings { get; set; }
        public virtual DbSet<Right> Rights { get; set; }
        public virtual DbSet<Role> Roles { get; set; }
        public virtual DbSet<SettingMaster> SettingMasters { get; set; }
        public virtual DbSet<UniqueSession> UniqueSessions { get; set; }
        public virtual DbSet<UserLog> UserLogs { get; set; }
        public virtual DbSet<UserProcessTime> UserProcessTimes { get; set; }
        public virtual DbSet<UserProjectMapping> UserProjectMappings { get; set; }
        public virtual DbSet<UserRole> UserRoles { get; set; }
        public virtual DbSet<User> Users { get; set; }
        public virtual DbSet<UserSession> UserSessions { get; set; }
        public virtual DbSet<UserVisit> UserVisits { get; set; }
        public virtual DbSet<VendorUserLoginDetail> VendorUserLoginDetails { get; set; }
    }
}
