using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Bll.Models
{
    public class MenuViewModel
    {
        public string Name { get; set; }
        public string Url { get; set; }
        public List<MenuViewModel> Children { get; set; }
    }
}
