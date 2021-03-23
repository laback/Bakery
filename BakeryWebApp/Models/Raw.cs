using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

#nullable disable

namespace BakeryWebApp
{
    public partial class Raw
    {
        [Key]
        public int RawId { get; set; }
        [Display(Name = "Наименование сырья")]
        [StringLength(50, MinimumLength = 3)]
        public string RawName { get; set; }

        public virtual ICollection<Norm> Norms { get; set; }
    }
}
