using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TaskManagerMVC
{
    public static class SqlConstants
    {
        // Stored procedure parameters
        public const string ParamTitle = "@Title";
        public const string ParamStatusId = "@StatusId";
        public const string ParamPriorityId = "@PriorityId";
        public const string ParamUserId = "@UserId";

        // Column names in result set
        public const string ColTaskId = "TaskId";
        public const string ColTitle = "Title";
        public const string ColDescription = "Description";
        public const string ColStartDate = "StartDate";
        public const string ColDueDate = "DueDate";
        public const string ColStatusName = "StatusName";
        public const string ColPriorityName = "PriorityName";
        public const string ColUserFullName = "UserFullName";
    }

}
