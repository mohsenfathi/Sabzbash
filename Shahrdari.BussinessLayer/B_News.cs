using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shahrdari.DataAccessLayer;
using Shahrdari.Enums;
using Shahrdari.Factory.Log;
using Shahrdari.Models;
using Shahrdari.Settings;
using System.Data.Entity;

namespace Shahrdari.BussinessLayer
{
    public class B_News : B_Base
    {
        public M_News Add(M_News News)
        {
            var db = new DatabaseContext();
            News = db.News.Add(News);
            db.SaveChanges();
            return News;
        }

        public void Edit(M_News News)
        {
            var db = new DatabaseContext();
            db.News.Where(c => c.Id == News.Id).Load();
            if (db.News.Local.Count == 0)
                return;
            db.News.Local[0].Description = News.Description;
            db.News.Local[0].Image = News.Image;
            db.News.Local[0].Summery = News.Summery;
            db.News.Local[0].Title = News.Title;
            db.SaveChanges();
        }

        public void Delete(int Id)
        {
            var db = new DatabaseContext();
            db.News.Where(c => c.Id == Id).Load();
            if (db.News.Local.Count == 0)
                return;
            db.News.Local.Remove(db.News.Local[0]);
            db.SaveChanges();
        }

        public M_News GetNews(int Id)
        {
            var db = new DatabaseContext();
            return db.News.Where(c => c.Id == Id).FirstOrDefault();
        }

        public List<M_News> GetNews()
        {
            return new DatabaseContext().News.ToList();
        }
    }
}
