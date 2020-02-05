using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Shahrdari.Models;
using Shahrdari.DataAccessLayer;
using Shahrdari.Enums;

namespace Shahrdari.BussinessLayer
{
    public class B_Pouriya_Date
    {
        public M_Pouriya_Date Add(DateTime Date)
        {
            var model = new M_Pouriya_Date { Date = Date };
            DatabaseContext db = new DatabaseContext();
            model = db.Pouriya_Date.Add(model);
            db.SaveChanges();
            return model;
        }

        public M_Pouriya_Date GetDate (int id)
        {
            return new DatabaseContext().Pouriya_Date.Where(c => c.Id == id).FirstOrDefault();
        }

        public M_Pouriya_Date GetDate(DateTime Date)
        {
            return new DatabaseContext().Pouriya_Date.Where(c => c.Date == Date).FirstOrDefault();
        }
    }
}
