using System;
using System.Collections.Generic;
using System.Text;

namespace DS.Bll.Context
{
    public class ConstantValue
    {
        //Template format.
        public const string EmployeeTemplate = "คุณ{0} {1}";

        //Datetime Format
        public const string DATETIME_YEARMONTHDAYTIME = "yyyyMMddHHmmss";
        public const string DATETIME_DAYMONTHYEAR = "dd/MM/yyyy";
        public const string DATETIME_DAYMONTHTEXTYEAR = "dd MMM yyyy";

        //Claims Type
        public const string CLAMIS_ORG = "Org";
        public const string CLAMIS_POS = "Pos";
        public const string CLAMIS_EMPNO = "EmpNo";

        //ValueHelp Where Clause.
        public const string VALUE_HELP_STATUS = "TRANSSTATUS";

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
