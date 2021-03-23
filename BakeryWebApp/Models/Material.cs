using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BakeryWebApp
{
    public partial class Material
    {
        [Key]
        public int MaterialId { get; set; }
        [Display(Name = "Наименование материала")]
        [StringLength(50, MinimumLength =3)]
        public string MaterialName { get; set; }

        public virtual ICollection<ProductsMaterial> ProductsMaterials { get; set; }
    }
}
