using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BakeryWebApp
{
    public partial class ProductsPlan
    {
        [Key]
        public int ProductPlan { get; set; }
        public int? DayPlanId { get; set; }
        public int? ProductId { get; set; }
        [Display(Name = "Количество изделий")]
        [Range(1, int.MaxValue)]
        public int? Count { get; set; }

        public virtual DayPlan DayPlan { get; set; }
        public virtual Product Product { get; set; }
    }
}
