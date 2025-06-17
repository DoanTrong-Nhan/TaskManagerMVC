namespace TaskManagerMVC.Helper
{
    public static class PermissionConstants
    {
        public const string CreateTaskMethod = "GET";
        public const string CreateTaskEndpoint = "/task/createtask";

        public const string UpdateTaskMethod = "GET"; // Form cập nhật
        public const string UpdateTaskEndpoint = "/task/update";

        public const string DeleteTaskMethod = "POST"; // Xóa task
        public const string DeleteTaskEndpoint = "/task/delete";

        public static readonly string WhitelistEndpoints = "/login";
        public static readonly string AccessDenied = "/Login/NotFound";
        public static readonly string LoginConst = "/Login/Index";
    }


}
