using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BakeryWebApp
{
    public partial class ProductsMaterial
    {
        [Key]
        public int ProductMaterial { get; set; }
        public int? MaterialId { get; set; }
        public int? ProductId { get; set; }
        [Display(Name = "Количество")]
        [Range(1, int.MaxValue)]
        public double? Quantity { get; set; }

        public virtual Material Material { get; set; }
        public virtual Product Product { get; set; }
    }
}
