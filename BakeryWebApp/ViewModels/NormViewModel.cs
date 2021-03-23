using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.ViewModels
{
    public class NormViewModel
    {
        public IEnumerable<Norm> Norms { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public SortViewModel SortViewModel { get; set; }
        public string RawName { get; set; }
        public string ProductionName { get; set; }
        public double Quantity { get; set; }
    }
}
