using D1WebApp.BusinessLogicLayer.ViewModels;
using D1WebApp.Models;
using D1WebApp.DataAccessLayer.Repositories;
using System;
using System.Data.Entity;
using System.Data.Common;
using System.Data.Entity.Core.EntityClient;
using System.Configuration;
using System.Data.SqlClient;

using D1WebApp.Controllers;
using System.DirectoryServices;
using System.IO;

using RestSharp;

namespace D1WebApp.Utilities
{
    public class ErrorLogs
    {
        public static void ErrorLog(Int64 UserID, String UserName, DateTime ErrorLogTimestamp, String ErrorType, String ErrorMessage, String Module, int IsError)
        {
            ErrorLogViewModel errorlogvm = new ErrorLogViewModel();
            errorlogvm.UserID = UserID;
            errorlogvm.UserName = UserName;
            errorlogvm.ErrorLogTimestamp = ErrorLogTimestamp;
            errorlogvm.ErrorType = ErrorType;
            errorlogvm.ErrorMessage = ErrorMessage;
            errorlogvm.Module = Module;
            errorlogvm.IsError = IsError;
            ErrorLogRepository errorlogerep = new ErrorLogRepository(new D1WebAppEntities());
            errorlogerep.InsertErrorLog(errorlogvm);
            errorlogerep.Save();
        }

        public string GenerateConnectionStringEntity(string connEntity)
        {
            // Initialize the SqlConnectionStringBuilder.  
            string dbServer = string.Empty;
            string dbName = string.Empty;
            // use it from previously built normal connection string  
            string connectString = Convert.ToString(ConfigurationManager.ConnectionStrings[connEntity]);
            var sqlBuilder = new SqlConnectionStringBuilder(connectString);
            // Set the properties for the data source.  
            dbServer = sqlBuilder.DataSource;
            dbName = sqlBuilder.InitialCatalog;
            sqlBuilder.UserID = "Database_User_ID";
            sqlBuilder.Password = "Database_User_Password";
            sqlBuilder.IntegratedSecurity = false;
            sqlBuilder.MultipleActiveResultSets = true;
            // Build the SqlConnection connection string.  
            string providerString = Convert.ToString(sqlBuilder);
            // Initialize the EntityConnectionStringBuilder.  
            var entityBuilder = new EntityConnectionStringBuilder();
            //Set the provider name.  
            entityBuilder.Provider = "System.Data.SqlClient";
            // Set the provider-specific connection string.  
            entityBuilder.ProviderConnectionString = providerString;
            // Set the Metadata location.  
            entityBuilder.Metadata = @"res://*/Models.ClientModel.csdl|  res: //*/Models.ClientModel.ssdl|  res: //*/Models.ClientModel.msl";
            return entityBuilder.ToString();
        }

        public static String BuildConnectionString(String Database)
        {
            try
            {
                var Sqlserver = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("SqlServer");
                var targetURL = D1WebApp.Utilities.GeneralConfiguration.GeneralConfiguration.CheckConfiguration("targetDirEcom");

                const string appName = "EntityFramework";
                const string providerName = "System.Data.SqlClient";
                string dataSource = Sqlserver.ConfigurationValue;
                string initialCatalog = targetURL.ConfigurationValue + Database + "\\" + Database + ".mdf";
                SqlConnectionStringBuilder sqlBuilder = new SqlConnectionStringBuilder();
                sqlBuilder.DataSource = dataSource;
                
                sqlBuilder.MultipleActiveResultSets = true;
                sqlBuilder.IntegratedSecurity = true;
                sqlBuilder.ApplicationName = appName;
                if (dataSource.Contains("DESKTOP-51FG8U2") || dataSource.Contains("LAPTOP-6SR7K8UO") || dataSource.Contains("DESKTOP-7RVH48O") || dataSource.Contains("localhost"))
                {                    
                    sqlBuilder.InitialCatalog = "ERP_TO_ECOMMERCE";
                }
                else
                {
                    sqlBuilder.AttachDBFilename = initialCatalog;
                    sqlBuilder.UserID = "sa";
                    sqlBuilder.Password = "Distone@123";
                }
                EntityConnectionStringBuilder efBuilder = new EntityConnectionStringBuilder();
                efBuilder.Metadata = "res://*/ClientModel.ClientModel.csdl|res://*/ClientModel.ClientModel.ssdl|res://*/ClientModel.ClientModel.msl";
                efBuilder.Provider = providerName;
                efBuilder.ProviderConnectionString = sqlBuilder.ConnectionString;
                return efBuilder.ConnectionString;

            }
            catch (Exception ed)
            {
                ErrorLogs.ErrorLog(0, "AddConfiguations", Miscellaneous.CurrentDateTime(), "", ed.Message + " And " + ed.InnerException, "AddConfiguations", 1);
                return ed.ToString();
            }
        }


    }

    public class CopyDir
    {
        public static void Copy(string sourceDirectory, string targetDirectory)
        {
            DirectoryInfo diSource = new DirectoryInfo(sourceDirectory);
            DirectoryInfo diTarget = new DirectoryInfo(targetDirectory);

            CopyAll(diSource, diTarget);
        }

        public static void CopyAll(DirectoryInfo source, DirectoryInfo target)
        {
            Directory.CreateDirectory(target.FullName);

            // Copy each file into the new directory.
            foreach (FileInfo fi in source.GetFiles())
            {
                Console.WriteLine(@"Copying {0}\{1}", target.FullName, fi.Name);
                fi.CopyTo(Path.Combine(target.FullName, fi.Name), true);
            }

            // Copy each subdirectory using recursion.
            foreach (DirectoryInfo diSourceSubDir in source.GetDirectories())
            {
                DirectoryInfo nextTargetSubDir =
                    target.CreateSubdirectory(diSourceSubDir.Name);
                CopyAll(diSourceSubDir, nextTargetSubDir);
            }
        }
    }
}
