using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BakeryWebApp.ViewModels
{
    public class RawViewModel
    {
        public IEnumerable<Raw> Raws { get; set; }
        public PageViewModel PageViewModel { get; set; }
        public string RawName { get; set; }
    }
}
