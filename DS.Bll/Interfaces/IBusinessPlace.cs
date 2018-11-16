using DS.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Bll.Interfaces
{
    public interface IBusinessPlace
    {
        IEnumerable<ValueHelpViewModel> GetBusinessPlace(string comCode);
        string GetOrgName(string comCode, string empNo);
    }
}
