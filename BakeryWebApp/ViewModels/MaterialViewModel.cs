using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.ViewModels
{
    public class MaterialViewModel
    {
        public IEnumerable<Material> Materials { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string MaterialName { get; set; }
    }
}
