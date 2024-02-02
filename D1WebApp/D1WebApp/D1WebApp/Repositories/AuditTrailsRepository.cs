using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.Models;
using D1WebApp.Utilities;
namespace D1WebApp.DataAccessLayer.Repositories
{
    public class AuditTrailsRepository : IAuditTrailsRepository,IDisposable
    {
        private D1WebAppEntities context;

        public AuditTrailsRepository()
        { }

        public AuditTrailsRepository(D1WebAppEntities context)
        {
            this.context = context;
        }

        public bool InsertAuditTrail(AuditTrailViewModel NewAudit)
        {
            try { 
            bool flag = false;
            AuditTrail RecordAudit = new AuditTrail();
            RecordAudit.ActionDate = NewAudit.ActionDate;
            RecordAudit.ActionType = NewAudit.ActionType;
            RecordAudit.RecordID = NewAudit.RecordID;
            RecordAudit.RecordType = NewAudit.RecordType;
            RecordAudit.UserID = Convert.ToInt64(NewAudit.UserID);
            context.AuditTrails.Add(RecordAudit);
            context.SaveChanges();
            flag = true;
            return flag;
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "InsertAuditTrailrepo", DateTime.Now, "InsertAuditTrailrepo", ed.ToString(), "InsertAuditTrailrepo", 2);
                return false;
            }
        }
        public AuditTrail GetAuditTrailRecord(long RecordID, int RecordType, int ActionType)
        {

            AuditTrail GetHistory = new AuditTrail();
            try { 
            GetHistory = context.AuditTrails.Where(cp => cp.RecordID == RecordID && cp.RecordType == RecordType && cp.ActionType == ActionType).OrderByDescending(cp => cp.AuditTrailID).FirstOrDefault();
            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "GetAuditTrailRecordrepo", DateTime.Now, "GetAuditTrailRecordrepo", ed.ToString(), "GetAuditTrailRecordrepo", 2);
             
            }
            return GetHistory;
        }
        public void Dispose()
        {
            context.Dispose();
            GC.SuppressFinalize(this);
        }
    }
}