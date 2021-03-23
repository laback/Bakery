using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BakeryWebApp
{
    public partial class ProductsProduction
    {
        [Key]
        public int ProductProductionId { get; set; }
        public int? DayProductionId { get; set; }
        public int? ProductId { get; set; }
        [Display(Name = "Количество")]
        [Range(1, int.MaxValue)]
        public int? Count { get; set; }

        public virtual DayProduction DayProduction { get; set; }
        public virtual Product Product { get; set; }
    }
}
