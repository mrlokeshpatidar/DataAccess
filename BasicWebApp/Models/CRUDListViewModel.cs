using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace BasicWebApp.Models
{
    public class CRUDListViewModel
    {
        public int id { get; set; }
        public string Name { get; set; }
        public string Address { get; set; }

        public List<CRUDViewModel> crud_lst { get; set; }
    }
}
