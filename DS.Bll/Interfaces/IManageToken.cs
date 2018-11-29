using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Bll.Interfaces
{
    public interface IManageToken
    {
        string AdUser { get; }
        string EmpNo { get; }
        string Position { get; }
        string Org { get; }
    }
}
