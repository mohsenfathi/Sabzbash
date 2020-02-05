using Shahrdari.DataAccessLayer;
using Shahrdari.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.SqlServer;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.BussinessLayer
{
    public class B_Location
    {
        public void Add(M_Location Location)
        {
            var db = new DatabaseContext();
            Location.IsOld = false;
            db.Location.Where(c => c.PersonelId == Location.PersonelId).Load();
            foreach (var li in db.Location.Local)
                li.IsOld = true;
            db.SaveChanges();
            db.Location.Add(Location);
            db.SaveChanges();
        }

        public List<M_Location> GetNearDrivers(double Lat,double Lng)
        {
            var startPoint = new { Lat = Lat, Lng = Lng };

            var closest = new DatabaseContext().Location.Where(x => (12742 * SqlFunctions.Asin(SqlFunctions.SquareRoot(SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (x.Lat - startPoint.Lat)) / 2) * SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (x.Lat - startPoint.Lat)) / 2) +
                                                SqlFunctions.Cos((SqlFunctions.Pi() / 180) * startPoint.Lat) * SqlFunctions.Cos((SqlFunctions.Pi() / 180) * (x.Lat)) *
                                                SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (x.Lng - startPoint.Lng)) / 2) * SqlFunctions.Sin(((SqlFunctions.Pi() / 180) * (x.Lng - startPoint.Lng)) / 2)))) <= 3).Where(x=>x.IsOld != true)
                                                .ToList();
            return closest;
        }

        public M_Location GetLocationByPersonelId(int PersonelId)
        {
            var db = new DatabaseContext();
            return db.Location.Where(x => x.PersonelId == PersonelId && x.IsOld == false).FirstOrDefault();
        }
    }
}
