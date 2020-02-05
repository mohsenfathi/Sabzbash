using Shahrdari.DataAccessLayer;
using Shahrdari.Enums;
using Shahrdari.Enums.Loging;
using Shahrdari.Factory.Log;
using Shahrdari.Models;
using Shahrdari.Settings;
using Shahrdari.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.BussinessLayer
{
    /// <summary>
    /// بیزینس مربوط به درخواست ها
    /// </summary>
    public class B_ServicesRequestItems : B_Base
    {
        /// <summary>
        /// افزودن مقدار به سیستم
        /// </summary>
        /// <param name="Item">ایتم مورد نظر</param>
        /// <returns>نتیجه تراکنش</returns>
        public M_ServicesRequestItems Add(M_ServicesRequestItems Item)
        {
            var db = new DatabaseContext();
            Item = db.ServicesRequestItems.Add(Item);
            db.SaveChanges();
            return Item;
        }

        /// <summary>
        /// دریافت مقادیر مربوط به ایتم های درخواست با توجه به شناسه درخواست
        /// </summary>
        /// <param name="RequestId">شناسه درخواست</param>
        /// <returns>نتیجه تراکنش</returns>
        public List<M_ServicesRequestItems> GetItems(int RequestId)
        {
            return new DatabaseContext().ServicesRequestItems.Where(c => c.RequestId == RequestId).ToList();
        }

        /// <summary>
        /// دریافت مقادیر مربوط به ایتم های درخواست با توجه به شناسه درخواست
        /// </summary>
        /// <param name="RequestId">شناسه درخواست</param>
        /// <param name="UserType">نوع کاربر</param>
        /// <returns>نتیجه تراکنش</returns>
        public List<M_ServicesRequestItems> GetItems(int RequestId, E_PublicCategory.SYSTEM_USER_TYPE UserType)
        {
            return new DatabaseContext().ServicesRequestItems.Where(c => c.RequestId == RequestId && c.UserType == UserType).ToList();
        }

        public List<M_ServicesRequestItems> GetItem(int Id, E_PublicCategory.SYSTEM_USER_TYPE UserType)
        {
            List<M_ServicesRequestItems> temp = new List<M_ServicesRequestItems>();
            var db = new DatabaseContext();
            db.ServicesRequests.Where(x => x.UserId == Id && x.Status == E_PublicCategory.REQUEST_STATUS.CLOSED && x.IsDeleted != true).Load();
            if(db.ServicesRequests.Local != null && db.ServicesRequests.Local.Count > 0)
                foreach(var li in db.ServicesRequests.Local)
                    temp.AddRange(db.ServicesRequestItems.Where(c => c.RequestId == li.Id && c.UserId == Id && c.UserType == UserType && c.IsFailed != true).ToList());
            temp.AddRange(db.ServicesRequestItems.Where(c => c.RequestId == -5001 && c.UserId == Id && c.UserType == UserType && c.IsFailed != true).ToList());
            return temp;
        }

        /// <summary>
        /// دریافت مجموع امتیاز های مربوط به یک درخواست
        /// </summary>
        /// <param name="RequestId">شناسه درخواست</param>
        /// <returns>نتیجه تراکنش</returns>
        public double GetItemsPoint(int RequestId)
        {
            var tmp = new DatabaseContext().ServicesRequestItems.Where(c => c.RequestId == RequestId).ToList();
            double result = 0;
            foreach (var li in tmp)
            {
                result += (double)(li.Value * li.ScorePerUnit);
            }
            return result;
        }

        public int GetSumUserPoint(int Id,E_PublicCategory.SYSTEM_USER_TYPE Type)
        {
            var ErnedPoints = GetItem(Id, Type);
            var LostPoints = new B_UserPayment().GetPayment(Id, Type);
            var ernedPoint = 0;
            if (ErnedPoints != null)
                foreach (var li in ErnedPoints)
                    ernedPoint += ((int)li.Value * (int)li.ScorePerUnit);
            var lostPoints = LostPoints != null ? (int)LostPoints.Sum(x => x.Point) : 0;
            if (ernedPoint != 0)
                return ernedPoint - lostPoints;
            else
                return 0;
        }

        public void CommitScoreItems(int RequestId, E_PublicCategory.SYSTEM_USER_TYPE UserType)
        {
            var db = new DatabaseContext();
            db.ServicesRequestItems.Where(c => c.RequestId == RequestId && c.UserType == UserType).Load();
            foreach (var li in db.ServicesRequestItems.Local)
            {
                li.IsFailed = false;
            }
            db.SaveChanges();
        }

        public void DennyScoreItems(int RequestId, E_PublicCategory.SYSTEM_USER_TYPE UserType)
        {
            var db = new DatabaseContext();
            db.ServicesRequestItems.Where(c => c.RequestId == RequestId && c.UserType == UserType).Load();
            foreach (var li in db.ServicesRequestItems.Local)
            {
                li.IsFailed = true;
            }
            db.SaveChanges();
        }

        /// <summary>
        /// حذف یک ایتم با توجه به شناسه ایتم
        /// </summary>
        /// <param name="Id"></param>
        public void Delete(int Id)
        {
            var db = new DatabaseContext();
            db.ServicesRequestItems.Remove(db.ServicesRequestItems.Where(c => c.Id == Id).FirstOrDefault());
            db.SaveChanges();
        }
    }
}
