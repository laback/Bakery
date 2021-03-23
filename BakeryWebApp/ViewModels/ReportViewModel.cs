using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.ViewModels
{
    public class ReportViewModel
    {
        public IEnumerable<Raw> Raws { get; set; }
        public IEnumerable<Material> Materials { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Число")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime DailyDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Число")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime BeginDate { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Число")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime EndDate { get; set; }
    }
}
