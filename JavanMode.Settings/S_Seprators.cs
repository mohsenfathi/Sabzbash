using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.Settings
{
    /// <summary>
    /// کدهای مربوط به حروف جداساز متن ها
    /// </summary>
    public static class S_Seprators
    {
        /// <summary>
        /// جداکننده متن خطا از کد ارور آن
        /// </summary>
        public static char ErrorMessageSeprator { get { return 'ᾎ'; } }

        /// <summary>
        /// جداکننده فیلد حاوی خطا از کد ارور آن
        /// </summary>
        public static char ErrorFieldNameSeprator { get { return 'Ɇ'; } }
    }
}
