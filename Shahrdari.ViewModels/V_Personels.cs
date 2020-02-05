using Shahrdari.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.ViewModels
{
    public class V_Personels : M_Personels
    {
        public string ConnectionId { get; set; }

        public long pKey { get; set; }
    }
}
