using DS.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Bll.Interfaces
{
    public interface IMenu
    {
        List<MenuViewModel> GenerateMenu(string username);
    }
}
