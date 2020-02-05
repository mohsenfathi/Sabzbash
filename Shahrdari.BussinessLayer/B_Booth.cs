using Shahrdari.DataAccessLayer;
using Shahrdari.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Device.Location;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.BussinessLayer
{
    public class B_Booth
    {
        /// <summary>
        /// دریافت غرفه ها
        /// </summary>
        /// <returns>لیستی از غرفه ها</returns>
        public List<M_BoothInfo> GetBoothes()
        {
            return new DatabaseContext().BoothInfo.ToList();
        }

        public List<M_BoothInfo> GetBoothes(double Lat, double Lng)
        {
            var startPoint = new { Lat = Lat, Lng = Lng };

            var closest = new DatabaseContext().BoothInfo.Where(x => (12742 * SqlFunctions.Asin(SqlFunctions.SquareRoot(SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (x.Lat - startPoint.Lat)) / 2) * SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (x.Lat - startPoint.Lat)) / 2) +
                                                SqlFunctions.Cos((SqlFunctions.Pi() / 180) * startPoint.Lat) * SqlFunctions.Cos((SqlFunctions.Pi() / 180) * (x.Lat)) *
                                                SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (x.Lng - startPoint.Lng)) / 2) * SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (x.Lng - startPoint.Lng)) / 2)))) <= 1.5)
                                                .ToList();
            return closest;
        }

        public M_BoothInfo GetBoothes(int BoothId)
        {
            return new DatabaseContext().BoothInfo.Where(x => x.Id == BoothId).FirstOrDefault();
        }

        public M_BoothInfo GetBoothByPersonelId(int PersonelId)
        {
            return new DatabaseContext().BoothInfo.Where(c => c.PersonelId == PersonelId).FirstOrDefault();
        }

        public M_BoothInfo Add(M_BoothInfo BoothInfo)
        {
            BoothInfo.IsDelete = false;
            var db = new DatabaseContext();
            BoothInfo = db.BoothInfo.Add(BoothInfo);
            db.SaveChanges();
            return BoothInfo;
        }

        public void Edit(M_BoothInfo BoothInfo)
        {
            var db = new DatabaseContext();
            db.BoothInfo.Where(c => c.Id == BoothInfo.Id).Load();
            db.BoothInfo.Local[0].Address = BoothInfo.Address;
            db.BoothInfo.Local[0].Capacity = BoothInfo.Capacity;
            db.BoothInfo.Local[0].Description = BoothInfo.Description;
            db.BoothInfo.Local[0].ImageName = BoothInfo.ImageName;
            db.BoothInfo.Local[0].Lat = BoothInfo.Lat;
            db.BoothInfo.Local[0].Lng = BoothInfo.Lng;
            db.BoothInfo.Local[0].Name = BoothInfo.Name;
            db.SaveChanges();
        }
    }
}
