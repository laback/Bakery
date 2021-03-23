using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.ViewModels
{
    public class ProductsPlansViewModel
    {
        public IEnumerable<ProductsPlan> ProductsPlans { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public string ProductName { get; set; }
        [DataType(DataType.Date)]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime PlanDate { get; set; }
        public int Count { get; set; }
        public ProductsPlan ProductsPlan { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Число")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime Date { get; set; }
    }
}
