using Shahrdari.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.BussinessLayer
{
    /// <summary>
    /// بیزینس مربوط به آمار ها
    /// </summary>
    public class B_Statistics
    {
        /// <summary>
        /// دریافت امار درخواست های یک ماهه اخیر
        /// </summary>
        /// <returns>ایتم اول تاریخ ایتم دوم تعداد درخواست</returns>
        public List<Tuple<string, int>> GetRequestStatisticsPastMonth()
        {
            var res = new List<Tuple<string, int>>();
            int daysCount = -30;
            var compDateOrg = DateTime.Now.AddDays(daysCount);
            var data = new DatabaseContext().ServicesRequests.Where(c => c.CreateDate >= compDateOrg)
                .Select(c => new { CreateDate = c.CreateDate.Year + "/" + c.CreateDate.Month + "/" + c.CreateDate.Day, Id = c.Id })
                .GroupBy(c => c.CreateDate).ToList();
            for (int i = daysCount; i <= 0; i++)
            {
                var dateComp = DateTime.Now.AddDays(i);
                var compDate = dateComp.Year + "/" + dateComp.Month + "/" + dateComp.Day;
                var tmpData = data.Where(c => c.Key == compDate).ToList();
                if (tmpData.Count() > 0)
                    res.Add(new Tuple<string, int>(DateTime.Now.AddDays(i).ConvertMiladiToShamsi("M/D"), tmpData[0].Count()));
                else
                    res.Add(new Tuple<string, int>(DateTime.Now.AddDays(i).ConvertMiladiToShamsi("M/D"), 0));
            }
            return res;
        }

        /// <summary>
        /// دریافت تعداد کاربران سیستم
        /// </summary>
        /// <returns>ایتم اول کاربران / ایتم دوم رانندگان / ایتم سوم مراکز تجمیع</returns>
        public Tuple<int, int, int> GetUsersCount()
        {
            var db = new DatabaseContext();
            return new Tuple<int, int, int>(
                db.Users.Count(),
                db.Personels.Where(c => c.PersonelType == Enums.E_PublicCategory.PERSONEL_TYPE.DRIVER).Count(),
                db.Personels.Where(c => c.PersonelType == Enums.E_PublicCategory.PERSONEL_TYPE.SUM_CENER).Count());
        }

        public int GetBoothCount()
        {
            return new DatabaseContext().Personels.Where(c => c.PersonelType == Enums.E_PublicCategory.PERSONEL_TYPE.SUM_CENER).Count();
        }
    }
}
