using Shahrdari.DataAccessLayer;
using Shahrdari.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.BussinessLayer
{
    /// <summary>
    /// منطق مربوط به بازخوردها
    /// </summary>
    public class B_CommentItems
    {
        /// <summary>
        /// افزودن بازخورد جدید
        /// </summary>
        /// <param name="Data">مدل مربوط به بازخورد</param>
        /// <returns>مدل اضاف شده</returns>
        public M_CommentItems Add(M_CommentItems Data)
        {
            var db = new DatabaseContext();
            Data = db.CommentItems.Add(Data);
            db.SaveChanges();
            return Data;
        }
    }
}
