using DS.Bll.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Bll.Interfaces
{
    public interface ICa
    {
        ValidationResultViewModel Add(CaViewModel model, IFormFileCollection file);
        CaViewModel Get(int id);
        IEnumerable<CaSearchViewModel> GetList();
    }
}
