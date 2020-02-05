using Shahrdari.DataAccessLayer;
using Shahrdari.Enums;
using Shahrdari.Factory.Log;
using Shahrdari.Models;
using Shahrdari.Settings;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.BussinessLayer
{
    public class B_Accounts : B_Base
    {
        public M_Accounts GetAccounts(int Id)
        {
            return new DatabaseContext().Accounts.Where(c => c.Id == Id).FirstOrDefault();
        }

        public bool AddAccount(M_Accounts Data)
        {
            Validate(Data);
            var db = new DatabaseContext();
            db.Accounts.Add(Data);
            db.SaveChanges();
            return true;
        }

        public List<M_Accounts> GetAccounts(int UserId,bool IsUser = true)
        {
            return new DatabaseContext().Accounts.Where(c => c.UserId == UserId && c.IsDelete != true).ToList();
        }

        public bool EditAccount(M_Accounts Data)
        {
            Validate(Data);
            var db = new DatabaseContext();
            db.Accounts.Where(x => x.Id == Data.Id).Load();
            if(db.Accounts.Local.Count == 0)
                throw F_ExeptionFactory.MakeExeption("حسابی برای ویرایش پیدا نشد",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Account", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.Accounts.Local[0].AccountHolderName = Data.AccountHolderName;
            db.Accounts.Local[0].BankName = Data.BankName;
            db.Accounts.Local[0].ShabaNumber = Data.ShabaNumber;
            db.SaveChanges();
            return true;
        }

        public bool DeleteAccount(int Id,int UserId)
        {
            var db = new DatabaseContext();
            db.Accounts.Where(x => x.Id == Id && x.UserId == UserId).Load();
            if(db.Accounts.Local.Count == 0)
                    throw F_ExeptionFactory.MakeExeption("حسابی برای حذف پیدا نشد",
                        ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Account", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.Accounts.Local[0].IsDelete = true;
            db.SaveChanges();
            return true;
        }
    }
}
