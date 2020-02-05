using Shahrdari.DataAccessLayer;
using Shahrdari.Models;
using System.Collections.Generic;
using System.Linq;

namespace Shahrdari.BussinessLayer
{
    /// <summary>
    /// مقادیر نقش ها
    /// </summary>
    public class B_PersonelRoleValues : B_Base
    {
        /// <summary>
        /// افزودن مقدار جدید برای نقش
        /// </summary>
        /// <param name="PersonelRoleValues">مقدار مورد نظر</param>
        /// <returns>مقدار افزوده شده</returns>
        public M_PersonelRoleValues Add(M_PersonelRoleValues PersonelRoleValues)
        {
            Validate(PersonelRoleValues);
            DatabaseContext db = new DatabaseContext();
            db.PersonelRoleValues.Add(PersonelRoleValues);
            db.SaveChanges();
            return PersonelRoleValues;
        }

        /// <summary>
        /// دریافت مقدارهای مربوط به نقش خاض
        /// </summary>
        /// <param name="PersonelRoleId">شنا نقش پدر</param>
        /// <returns>مقادیر نقش</returns>
        public List<M_PersonelRoleValues> GetPersonelRoleValues(int PersonelRoleId)
        {
            DatabaseContext db = new DatabaseContext();
            return db.PersonelRoleValues.Where(c=>c.PersonelRoleId == PersonelRoleId).ToList();
        }

        /// <summary>
        /// حذف مقدارهای مربوط به نقش خاض
        /// </summary>
        /// <param name="PersonelRoleId">شنا نقش پدر</param>
        public void Delete(int PersonelRoleId)
        {
            DatabaseContext db = new DatabaseContext();
            db.PersonelRoleValues.RemoveRange(db.PersonelRoleValues.Where(c => c.PersonelRoleId == PersonelRoleId).ToList());
            db.SaveChanges();
        }
    }
}
