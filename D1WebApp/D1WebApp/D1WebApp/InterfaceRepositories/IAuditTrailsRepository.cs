using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using D1WebApp.Models;
using D1WebApp.BusinessLogicLayer.ViewModels;
namespace D1WebApp.DataAccessLayer.Repositories
{
    public interface IAuditTrailsRepository : IDisposable
    {
        bool InsertAuditTrail(AuditTrailViewModel NewAudit);
        AuditTrail GetAuditTrailRecord(long RecordID, int RecordType, int ActtionType);
    }
}