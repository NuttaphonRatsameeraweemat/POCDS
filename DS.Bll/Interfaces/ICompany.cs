using DS.Bll.Models;
using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Bll.Interfaces
{
    public interface ICompany
    {
        IEnumerable<ValueHelpViewModel> GetCompanyByEmp(string empNo);
    }
}
