using Shahrdari.DataAccessLayer;
using Shahrdari.Enums;
using Shahrdari.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.BussinessLayer
{
    /// <summary>
    /// بیزینس مربوط به مقادیر لوکاپ
    /// </summary>
    public class B_PublicCategory : B_Base
    {
        /// <summary>
        /// دریافت مقادیر لوکاپ با توجه به والد
        /// </summary>
        /// <param name="ParentType">والد</param>
        /// <returns>نتیجه تراکنش</returns>
        public List<M_PublicCategory> GetPublicCategory(E_PublicCategory.PUBLIC_CATEGORY_PARENT ParentType)
        {
            return new DatabaseContext().PublicCategory.Where(c => c.ParentId == (int)ParentType).ToList();
        }

        public string GetCategoryTitle(int Id)
        {
            return new DatabaseContext().PublicCategory.Where(x => x.Id == Id).Select(x => x.Title).FirstOrDefault();
        }
    }
}
