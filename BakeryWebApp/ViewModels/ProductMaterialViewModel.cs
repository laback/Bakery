using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.ViewModels
{
    public class ProductMaterialViewModel
    {
        public IEnumerable<ProductsMaterial> ProductsMaterials { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public string MaterialName { get; set; }
        public string ProductionName { get; set; }
        public double Quantity { get; set; }
    }
}
