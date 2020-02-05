using Shahrdari.DataAccessLayer;
using Shahrdari.Factory.Log;
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
    /// بیزینس مربوط به دسته بندی هت
    /// </summary>
    public class B_ServicesCategories : B_Base
    {
        /// <summary>
        /// دریافت دسته بندی های موجود
        /// </summary>
        /// <param name="OnlyIsActives">فقط دسته بندی های فعال بیاییند ؟</param>
        /// <returns>دسته بندی های مورد نظر</returns>
        public List<M_ServicesCategories> GetServicesCategories(bool OnlyIsActives)
        {
            DatabaseContext db = new DatabaseContext();
            if (OnlyIsActives)
                return db.ServicesCategories.Where(c => c.IsActive == true).ToList();
            else
                return db.ServicesCategories.ToList();
        }

        /// <summary>
        /// دریافت دسته بندی با توجه به شناسه والد
        /// </summary>
        /// <param name="ParentId">شناسه والد</param>
        /// <param name="OnlyIsActives">فقط دسته بندی های فعال بیاییند ؟</param>
        /// <returns>دسته بندی های مورد نظر</returns>
        public List<M_ServicesCategories> GetServicesCategories(int ParentId, bool OnlyIsActives)
        {
            DatabaseContext db = new DatabaseContext();
            if (OnlyIsActives)
                return db.ServicesCategories.Where(c => c.IsActive == true && c.ParentId == ParentId).ToList();
            else
                return db.ServicesCategories.Where(c => c.ParentId == ParentId).ToList();
        }

        /// <summary>
        /// دریافت یک دسته بندی با توجه به شناسه
        /// </summary>
        /// <param name="Id">شناسه</param>
        /// <returns>دسته بندی مورد نظر</returns>
        public M_ServicesCategories GetServicesCategories(int Id)
        {
            return new DatabaseContext().ServicesCategories.Where(c => c.Id == Id).FirstOrDefault();
        }

        /// <summary>
        /// دریافت دسته بندی با توجه به مجموعه ای از شناسه ها
        /// </summary>
        /// <param name="Ids">شناسه ها</param>
        /// <returns>دسته بندی های مورد نظر</returns>
        public List<M_ServicesCategories> GetServicesCategories(List<int> Ids)
        {
            return new DatabaseContext().ServicesCategories.Where(c => Ids.Contains(c.Id)).ToList();
        }
        
        /// <summary>
        /// افزودن دسته بندی
        /// </summary>
        /// <param name="ServicesCategories">دسته بندی مورد نظر</param>
        /// <returns>دسته بندی افزوده شده</returns>
        public M_ServicesCategories Add(M_ServicesCategories ServicesCategories)
        {
            Validate(ServicesCategories);
            DatabaseContext db = new DatabaseContext();
            ServicesCategories.CreateDate = DateTime.Now;
            if (ServicesCategories.IsActive == false)
                ServicesCategories.DeactiveDate = DateTime.Now;
            db.ServicesCategories.Add(ServicesCategories);
            db.SaveChanges();
            return ServicesCategories;
        }

        /// <summary>
        /// ویرایش یک دسته بندی
        /// </summary>
        /// <param name="ServicesCategories">دسته بندی برای ویرایش</param>
        public void Edit(M_ServicesCategories ServicesCategories)
        {
            Validate(ServicesCategories);
            if (!ServicesCategories.IsActive)
                ServicesCategories.DeactiveDate = DateTime.Now;
            DatabaseContext db = new DatabaseContext();
            db.ServicesCategories.Where(c => c.Id == ServicesCategories.Id).Load();
            updateChild(db.ServicesCategories.Local[0].Route, ServicesCategories.Route, db.ServicesCategories.Local[0].Lineages, ServicesCategories.Lineages, ServicesCategories.Id);
            db.ServicesCategories.Local[0].Description = ServicesCategories.Description;
            db.ServicesCategories.Local[0].HaveNew = ServicesCategories.HaveNew;
            db.ServicesCategories.Local[0].ImageName = ServicesCategories.ImageName;
            db.ServicesCategories.Local[0].IsActive = ServicesCategories.IsActive;
            db.ServicesCategories.Local[0].DeactiveDate = ServicesCategories.DeactiveDate;
            db.ServicesCategories.Local[0].Lineages = ServicesCategories.Lineages;
            db.ServicesCategories.Local[0].ParentId = ServicesCategories.ParentId;
            db.ServicesCategories.Local[0].Route = ServicesCategories.Route;
            db.ServicesCategories.Local[0].Title = ServicesCategories.Title;
            db.ServicesCategories.Local[0].Unit = ServicesCategories.Unit;
            db.ServicesCategories.Local[0].ScorePerUnit = ServicesCategories.ScorePerUnit;
            db.ServicesCategories.Local[0].ScorePerUnitDriver = ServicesCategories.ScorePerUnitDriver;
            db.SaveChanges();
        }

        /// <summary>
        /// حذف یم دسته بندی
        /// </summary>
        /// <param name="Id">شناسه دسته بندی</param>
        public void Delete(int Id)
        {
            DatabaseContext db = new DatabaseContext();
            if (db.ServicesCategories.Where(c => c.ParentId == Id).Count() > 0)
                throw F_ExeptionFactory.MakeExeption($"برای این دسته بندی زیر شاخه وجود دارد", Enums.Loging.E_LogType.SYSTEM_ERROR);
            //if (db.ServicesRequests.Where(c => c.ServicesCategoryId == Id).Count() > 0)
            //    throw F_ExeptionFactory.MakeExeption($"برای این دسته بندی درخواست ثبت شده", Enums.Loging.E_LogType.SYSTEM_ERROR);
            db.ServicesCategories.Remove(db.ServicesCategories.Where(c => c.Id == Id).FirstOrDefault());
            db.SaveChanges();
        }

        private void updateChild(string oldRouteValue, string newRouteValue, string oldLineageValue, string newLineageValue, int parentId)
        {
            DatabaseContext db = new DatabaseContext();
            db.ServicesCategories.Where(c => c.ParentId == parentId).Load();
            foreach (var li in db.ServicesCategories.Local)
            {
                li.Route = li.Route.Replace(oldRouteValue, newRouteValue);
                li.Lineages = li.Lineages.Replace(oldLineageValue, newLineageValue);
                updateChild(oldRouteValue, newRouteValue, oldLineageValue, newLineageValue, li.Id);
            }
            db.SaveChanges();
        }

        public int? GetParentId(string Title)
        {
            var db = new DatabaseContext();
            db.ServicesCategories.Where(x => x.Title == Title).Load();
            if (db.ServicesCategories.Local.Count > 0)
                return db.ServicesCategories.Local[0].ParentId;
            else
                return null;
        }
    }
}
