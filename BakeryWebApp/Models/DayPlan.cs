using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BakeryWebApp
{
    public partial class DayPlan
    {
        [Key]
        public int DayPlanId { get; set; }
        [DataType(DataType.Date)]
        [Display(Name = "Число")]
        [DisplayFormat(DataFormatString = "{0:dd'/'MM'/'yyyy}", ApplyFormatInEditMode = true)]
        public DateTime? Date { get; set; }

        public virtual ICollection<ProductsPlan> ProductsPlans { get; set; }
    }
}
