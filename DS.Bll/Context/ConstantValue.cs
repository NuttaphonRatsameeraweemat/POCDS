using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Bll.Context
{
    public class ConstantValue
    {
        //Template format.
        public const string EmployeeTemplate = "คุณ{0} {1}";
        public const string DateTimeFormat = "yyyyMMddHHmmssfff";

        //Claims Type
        public const string CLAMIS_ORG = "Org";
        public const string CLAMIS_POS = "Pos";
        public const string CLAMIS_EMPNO = "EmpNo";

        //Role and Menu.
        public const string RootMenuCode = "ROOT";
        public const string RoleEveryone = "Role_Everyone";
        public const string RoleSuperAdmin = "Role_Super_Admin";

        //Configuration, record status.
        public const string ConfigStatusActive = "ACTIVE";

        //Status.
        public const string TransStatusSaved = "SAVEREQUEST";

        //Elastic index and type.
        public const string CAIndex = "ca_index";
        public const string CAType = "ca_type";

    }
}
