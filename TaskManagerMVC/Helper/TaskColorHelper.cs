namespace TaskManagerMVC.Helpers
{
    public static class TaskColorHelper
    {
        public static string GetStatusBadgeClass(string statusName)
        {
            return statusName switch
            {
                "To Do" => "bg-info",
                "In Progress" => "bg-primary",
                "In Review" => "bg-warning text-dark",
                "Completed" => "bg-success",
                "Blocked" => "bg-danger",
                _ => "bg-secondary"
            };
        }

        public static string GetPriorityBadgeClass(string priorityName)
        {
            return priorityName switch
            {
                "Critical" => "bg-danger text-white",
                "High" => "bg-danger",
                "Medium" => "bg-warning text-dark",
                "Low" => "bg-success",
                "Trickle" => "bg-secondary",
                _ => "bg-secondary"
            };
        }
    }
}
