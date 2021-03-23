using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BakeryWebApp
{
    public partial class Norm
    {
        [Key]
        public int NormId { get; set; }
        public int? RowId { get; set; }
        public int? ProductId { get; set; }
        [Display(Name = "Количество")]
        [Range(1,int.MaxValue)] 
        public double? Quantity { get; set; }

        public virtual Product Product { get; set; }
        public virtual Raw Raw { get; set; }
    }
}
