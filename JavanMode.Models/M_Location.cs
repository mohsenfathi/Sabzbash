using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Shahrdari.Models
{
    [Table("Location")]
    public class M_Location
    {
        [Key]
        [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
        public int Id { get; set; }
        public double Lat { get; set; }
        public double Lng { get; set; }
        public double Bearing { get; set; }
        public int PersonelId { get; set; }
        public DateTime Date { get; set; }
        public bool IsOld { get; set; }
    }
}
