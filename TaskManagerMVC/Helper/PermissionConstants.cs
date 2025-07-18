﻿namespace TaskManagerMVC.Helper
{
    public static class PermissionConstants
    {
        public const string GET_METHOD = "GET";
        public const string CREATE_TASK_ENDPOINT = "/task/createtask";

        public const string UPDATE_TASK_ENDPOINT = "/task/update";

        public const string POST_METHOD = "POST"; // Xóa task
        public const string DELETE_TASK_ENDPOINT = "/task/delete";

        public const string WHITELIST_ENDPOINTS = "/login";
        public const string ACCESS_DENIED_ENDPOINT = "/login/notfound"; 

        public const string LOGIN_ENDPOINT = "/login/index";
    }
}