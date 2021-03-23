using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BakeryWebApp
{
    public partial class Product
    {
        [Key]
        public int ProductId { get; set; }
        [Display(Name = "Наименование изделия")]
        [StringLength(50, MinimumLength =3)]
        public string ProductName { get; set; }

        public virtual ICollection<Norm> Norms { get; set; }
        public virtual ICollection<ProductsMaterial> ProductsMaterials { get; set; }
        public virtual ICollection<ProductsPlan> ProductsPlans { get; set; }
        public virtual ICollection<ProductsProduction> ProductsProductions { get; set; }
    }
}
