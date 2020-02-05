using Shahrdari.DataAccessLayer;
using Shahrdari.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.BussinessLayer
{
    public class B_CarInfo
    {
        /// <summary>
        /// دریافت خودرو و راننده
        /// </summary>
        /// <returns>لیستی از خودرو و راننده</returns>
        public List<M_CarInfo> GetCarsInfo()
        {
            return new DatabaseContext().CarInfo.ToList();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="PersonelId"></param>
        /// <returns></returns>
        public M_CarInfo GetCarInfo(int PersonelId)
        {
            var user = new B_Personels().GetPersonelById(PersonelId);
            if (user != null)
                return new DatabaseContext().CarInfo.Where(x => x.PersonelId == PersonelId && user.IsActive == true && user.IsDeleted == false).FirstOrDefault();
            else
                return null;
        }

        public M_CarInfo GetCarInfoByPersonelId(int PersonelId)
        {
            return new DatabaseContext().CarInfo.Where(c => c.PersonelId == PersonelId).FirstOrDefault();
        }

        public M_CarInfo Add(M_CarInfo CarInfo)
        {
            var db = new DatabaseContext();
            CarInfo = db.CarInfo.Add(CarInfo);
            db.SaveChanges();
            return CarInfo;
        }

        public void Edit(M_CarInfo CarInfo)
        {
            var db = new DatabaseContext();
            db.CarInfo.Where(c => c.Id == CarInfo.Id).Load();
            db.CarInfo.Local[0].Capacity = CarInfo.Capacity;
            db.CarInfo.Local[0].Color = CarInfo.Color;
            db.CarInfo.Local[0].Image = CarInfo.Image;
            db.CarInfo.Local[0].Name = CarInfo.Name;
            db.CarInfo.Local[0].TagColor = CarInfo.TagColor;
            db.CarInfo.Local[0].TagFirst = CarInfo.TagFirst;
            db.CarInfo.Local[0].TagLast = CarInfo.TagLast;
            db.CarInfo.Local[0].TagMiddle = CarInfo.TagMiddle;
            db.CarInfo.Local[0].TagNational = CarInfo.TagNational;
            db.CarInfo.Local[0].Type = CarInfo.Type;
            db.SaveChanges();
        }
    }
}
