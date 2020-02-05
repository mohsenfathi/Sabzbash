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
    public class B_ListenEar
    {
        public bool AddListenEar(M_ListenEar Data)
        {
            if (string.IsNullOrEmpty(Data.Description))
                throw F_ExeptionFactory.MakeExeption("توضیحات را وارد کنید",
                    ((int)E_ErrorCodes.NOT_FOUND) + S_Seprators.ErrorFieldNameSeprator.ToString() + "Description", Enums.Loging.E_LogType.SYSTEM_ERROR);
            var db = new DatabaseContext();
            db.ListenEar.Add(Data);
            db.SaveChanges();
            return true;
        }

        public List<M_ListenEar> GetAllListenEar()
        {
            return new DatabaseContext().ListenEar.OrderByDescending(c => c.Id).ToList();
        }

        public void UpdateRead()
        {
            var db = new DatabaseContext();
            db.ListenEar.Where(c => c.IsRead == false).Load();
            foreach (var li in db.ListenEar.Local)
                li.IsRead = true;
            db.SaveChanges();
        }
    }
}
