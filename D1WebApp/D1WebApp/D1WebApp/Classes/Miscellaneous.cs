using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Web;
using D1WebApp.DataAccessLayer.Repositories;
using D1WebApp.Models;
using D1WebApp.BusinessLogicLayer.ViewModels;
using System.IO;
using D1WebApp.BusinessLogicLayer;
using System.Drawing;
using System.Text;
using System.Security.Cryptography;

namespace D1WebApp.Utilities
{
    public class Miscellaneous
    {
        public enum UserSessionValidate
        {
            Expired_Session = 0,
            Extend_Session = 1,
        }
        public enum VerifyStatus
        {
            Verified = 1,
            UnVerified = 0,
        }
        public enum UserType
        {
            Admin = 1,
            Client = 0,
        }



       


        public static decimal GetRandomCode(int length)
        {
            Random random = new Random();
            return Convert.ToDecimal(random.Next(1, 1000000000).ToString().Substring(0, length));
        }



        public static long GetRandomCodeNo(int length1, int length2)
        {
            Random random = new Random();
            string one = random.Next(1, 1000000000).ToString().Substring(0, length1);
            string two = random.Next(1, 1000000000).ToString().Substring(0, length2);
            string join = one + two;
            return Convert.ToInt64(join);
        }
      
        public static bool InsertAuditTrails(long RecordID, long UserID, int ActionType, DateTime RecordDate, int RecordType)
        {
            bool flag = false;
            AuditTrailsRepository AuditRepo = new AuditTrailsRepository(new D1WebAppEntities());

            AuditTrailViewModel audittrailvm = new AuditTrailViewModel();
            audittrailvm.RecordID = RecordID;
            if (UserID == 0)
            {
                audittrailvm.UserID = 2;
            }
            else
            {
                audittrailvm.UserID = UserID;
            }
            audittrailvm.ActionType = ActionType;
            audittrailvm.ActionDate = RecordDate;
            audittrailvm.RecordType = RecordType;

            AuditRepo.InsertAuditTrail(audittrailvm);
            flag = true;
            return flag;

        }
        public static AuditTrail GetRecordHistory(long RecordID, int RecordType, int ActionType)
        {
            AuditTrail GetHistory = new AuditTrail();
            AuditTrailsRepository AuditTrailsRepo = new AuditTrailsRepository(new D1WebAppEntities());
            GetHistory = AuditTrailsRepo.GetAuditTrailRecord(RecordID, RecordType, ActionType);
            return GetHistory;

        }
        public static bool InsertAuditTrails(long RecordID, long UserID, int ActionType, DateTime RecordDate, int RecordType, D1WebAppEntities context)
        {
            bool flag = false;
            AuditTrailsRepository AuditRepo = new AuditTrailsRepository(context);

            AuditTrailViewModel audittrailvm = new AuditTrailViewModel();
            audittrailvm.RecordID = RecordID;
            if (UserID == 0)
            {
                audittrailvm.UserID = 2;
            }
            else
            {
                audittrailvm.UserID = UserID;
            }
            audittrailvm.ActionType = ActionType;
            audittrailvm.ActionDate = RecordDate;
            audittrailvm.RecordType = RecordType;

            AuditRepo.InsertAuditTrail(audittrailvm);
            flag = true;
            return flag;

        }
        public static Nullable<Decimal> StringToDecimal(String Value)
        {
            try
            {
                if (Value == "")
                    return null;
                else if (Value == null)
                    return null;
                else
                    return Convert.ToDecimal(Value);
            }
            catch (Exception)
            {
                return null;
            }

        }
        public static long StringToLong(String Value)
        {
            try
            {
                if (Value == "")
                    return 0;
                else if (Value == null)
                    return 0;
                else
                    return Convert.ToInt64(Value);
            }
            catch (Exception)
            {
                return 0;
            }

        }
        public static string RemoveNull(string Value)
        {
            if (!string.IsNullOrEmpty(Value))
                return Value;
            else
                return "";
        }
        public static string NumberToWords(Int64 number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }

        public static List<string> CompareRecords<T>(T PreviousRecord, T CurrentRecord)
        {
            List<string> ColumnsList = new List<string>();

            if (PreviousRecord != null)
            {
                PropertyInfo[] ArchiveColumns;
                ArchiveColumns = typeof(T).GetProperties();

                Type Type = CurrentRecord.GetType();
                IList<PropertyInfo> CAprops = new List<PropertyInfo>(Type.GetProperties());

                for (int i = 1; i < ArchiveColumns.Length; i++)
                {
                    if (Convert.ToString(ArchiveColumns[i].GetValue(PreviousRecord, null)) != Convert.ToString(ArchiveColumns[i].GetValue(CurrentRecord, null)))
                    {
                        ColumnsList.Add(ArchiveColumns[i].Name.ToString());
                    }
                }
            }

            return ColumnsList;
        }

        public static void CreateDirectory(string path)
        {
            bool folderExists = Directory.Exists(path);
            if (!folderExists)
                Directory.CreateDirectory(path);
        }

        public static DateTime CurrentDateTime()
        {
            return DateTime.UtcNow.AddHours(5).AddMinutes(30);
        }


        public enum ActionType
        {
            Created = 1,
            Modified = 2,
            Published = 3,
            Rejected = 4,
            Deleted = 5,
        }


        public enum RecordTypeForAuditTrail
        {
            user = 1,

        }




        public enum RecordStatus
        {
            Expired = 5,
            Normal = 4,
            Pending = 1,
            Published = 3,
            RegistrationInComplete = 0,
            Rejected = 2,
        }
        public enum MessageType
        {
            Success = 1,
            Failed = 2,
            Error = 3,
            Login = 4,
            Warning = 5
        }
        public bool CheckSizeOfImage(string ImageSize, MemoryStream ImageFile)
        {
            bool flag = false;
            double width = 0;
            double height = 0;
            string[] getSize = ImageSize.Split('x');
            width = Convert.ToInt64(getSize[0]);
            height = Convert.ToInt64(getSize[1]);
            var getImage = Image.FromStream(ImageFile);
            double checkwidth = width;
            double checkheight = height;
            if (getImage.Width >= checkwidth && getImage.Height >= checkheight)
            {
                flag = true;
            }

            return flag;
        }


        public static Message GetMessage(long ID, string messagetype, string messagedesc, List<long> BuyOrderIds = null, long GroupID = 0)
        {
            Message message = new Message();
            message.ID = ID;
            message.GroupNo = GroupID;
            message.MessageType = messagetype;
            message.MessageDescription = messagedesc;

            return message;
        }

        public static string getsplitedstring(string value)
        {
            string getnewstring = "";
            List<char> getstring = new List<char>();
            getstring = value.Split(' ').Select(y => y[0]).ToList();
            for (int i = 0; i < getstring.Count(); i++)
            {
                getnewstring = string.Concat(getnewstring, " ", getstring[i]);
            }
            return getnewstring;
        }





    }

   public class EasyMD5
    {
        private static string GetMd5Hash(byte[] data)
        {
            StringBuilder sBuilder = new StringBuilder();
            for (int i = 0; i < data.Length; i++)
                sBuilder.Append(data[i].ToString("x2"));
            return sBuilder.ToString();
        }

        private static bool VerifyMd5Hash(byte[] data, string hash)
        {
            return 0 == StringComparer.OrdinalIgnoreCase.Compare(GetMd5Hash(data), hash);
        }

        public static string Hash(string data)
        {
            using (var md5 = MD5.Create())
                return GetMd5Hash(md5.ComputeHash(Encoding.UTF8.GetBytes(data)));
        }
        public static string Hash(FileStream data)
        {
            using (var md5 = MD5.Create())
                return GetMd5Hash(md5.ComputeHash(data));
        }

        public static bool Verify(string data, string hash)
        {
            using (var md5 = MD5.Create())
                return VerifyMd5Hash(md5.ComputeHash(Encoding.UTF8.GetBytes(data)), hash);
        }

        public static bool Verify(FileStream data, string hash)
        {
            using (var md5 = MD5.Create())
                return VerifyMd5Hash(md5.ComputeHash(data), hash);
        }
    }
}