using Shahrdari.Factory.Log;
using Shahrdari.DataAccessLayer;
using Shahrdari.Enums.Loging;
using Shahrdari.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.BussinessLayer
{
    /// <summary>
    /// بیزینس مربوط به نقش ها
    /// </summary>
    public class B_PersonelRoles : B_Base
    {
        /// <summary>
        /// افزودن نقش جدید
        /// </summary>
        /// <param name="PersonelRole">نقش مورد نظر</param>
        /// <returns>نقش اضافه شده</returns>
        public M_PersonelRoles Add(M_PersonelRoles PersonelRole)
        {
            Validate(PersonelRole);
            DatabaseContext db = new DatabaseContext();
            db.PersonelRoles.Add(PersonelRole);
            db.SaveChanges();
            return PersonelRole;
        }

        /// <summary>
        /// دریافت همه نقش ها
        /// </summary>
        /// <returns>نقش ها</returns>
        public List<M_PersonelRoles> GetPersonelRoles()
        {
            DatabaseContext db = new DatabaseContext();
            return db.PersonelRoles.ToList();
        }

        /// <summary>
        /// دریافت نقش با توجه به شنا آن
        /// </summary>
        /// <param name="Id">شناسه نقش</param>
        /// <returns>نقش مورد نظر</returns>
        public M_PersonelRoles GetPersonelRoles(int Id)
        {
            DatabaseContext db = new DatabaseContext();
            return db.PersonelRoles.Where(c => c.Id == Id).FirstOrDefault();
        }

        /// <summary>
        /// ویرایش نقش
        /// </summary>
        /// <param name="PersonelRole">نقش مورد نظر برای ویرایش</param>
        public void Edit(M_PersonelRoles PersonelRole)
        {
            DatabaseContext db = new DatabaseContext();
            db.PersonelRoles.Where(c => c.Id == PersonelRole.Id).Load();
            db.PersonelRoles.Local[0].Title = PersonelRole.Title;
            db.PersonelRoles.Local[0].HasFullControl = PersonelRole.HasFullControl;
            db.SaveChanges();
        }

        /// <summary>
        /// حذف نقش
        /// </summary>
        /// <param name="Id">شناسه نقش</param>
        public void Delete(int Id)
        {
            if (new B_Personels().GetPersonels(Id).Count > 0)
                throw F_ExeptionFactory.MakeExeption("این نقش استفاده شده و امکان حذف وجود ندارد", E_LogType.SYSTEM_ERROR);
            B_PersonelRoleValues bPersonelRoleValue = new B_PersonelRoleValues();
            bPersonelRoleValue.Delete(Id);
            DatabaseContext db = new DatabaseContext();
            db.PersonelRoles.Remove(db.PersonelRoles.Where(c => c.Id == Id).FirstOrDefault());
            db.SaveChanges();
        }
    }
}
