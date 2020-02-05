using Shahrdari.DataAccessLayer;
using Shahrdari.Enums;
using Shahrdari.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.BussinessLayer
{
    public class B_UserPayment : B_Base
    {
        public bool AddUserPayment(M_UserPayment Datas)
        {
            var db = new DatabaseContext();
            db.UserPayment.Add(Datas);
            db.SaveChanges();
            return true;
        }

        public List<M_UserPayment> GetPayment(int UserId)
        {
            return new DatabaseContext().UserPayment.Where(c => c.UserId == UserId && c.UserType == E_PublicCategory.SYSTEM_USER_TYPE.NORMAL_USER).ToList();
        }
        
        public M_UserPayment GetPaymentById(int Id)
        {
            return new DatabaseContext().UserPayment.Where(c => c.Id == Id).FirstOrDefault();
        }

        public List<M_UserPayment> GetPayment(int PageNumber, E_PublicCategory.PAYMENT_TYPE? Type, E_PublicCategory.PAYMENT_STATUS? Status)
        {
            return new DatabaseContext().UserPayment
                .Where(c => (Type == null || c.Type == Type.Value) && (Status == null || c.Status == Status.Value))
                .OrderByDescending(c=>c.Id)
                .Skip((PageNumber - 1) * PageCapacity)
                .Take(PageCapacity)
                .ToList();
        }

        public M_UserPayment ChangeState(E_PublicCategory.PAYMENT_STATUS Status , int PaymentId)
        {
            var db = new DatabaseContext();
            db.UserPayment.Where(c => c.Id == PaymentId).Load();
            if (db.UserPayment.Local.Count == 0)
                return new M_UserPayment();
            db.UserPayment.Local[0].Status = Status;
            db.SaveChanges();
            return db.UserPayment.Local[0];
        }

        public List<M_UserPayment> GetPayment(int UserId,E_PublicCategory.SYSTEM_USER_TYPE Type)
        {
            return new DatabaseContext().UserPayment.Where(c => c.UserId == UserId && c.UserType == Type).ToList();
        }

        public int GetPaymentCuontByStatus(E_PublicCategory.PAYMENT_STATUS Status)
        {
            return new DatabaseContext().UserPayment.Where(c => c.Status == Status).Count();
        }

        public List<Tuple<int, E_PublicCategory.PAYMENT_STATUS>> GetForChartStatus()
        {
            var db = new DatabaseContext();
            var res = db.UserPayment
                .Select(c => new { Status = c.Status, c.Id })
                .GroupBy(c => c.Status)
                .Select(c => new { Count = c.Count(), Type = c.Key })
                .ToList();
            return res.Select(c => new Tuple<int, E_PublicCategory.PAYMENT_STATUS>(c.Count, c.Type)).ToList();
        }

        public List<Tuple<int, E_PublicCategory.PAYMENT_TYPE>> GetForChartType()
        {
            var db = new DatabaseContext();
            var res = db.UserPayment
                .Select(c => new { Type = c.Type, c.Id })
                .GroupBy(c => c.Type)
                .Select(c => new { Count = c.Count(), Type = c.Key })
                .ToList();
            return res.Select(c => new Tuple<int, E_PublicCategory.PAYMENT_TYPE>(c.Count, c.Type)).ToList();
        }
    }
}
