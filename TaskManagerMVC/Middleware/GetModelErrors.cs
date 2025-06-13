using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace TaskManagerAPI.Middleware
{
    public static class GetModelErrors
    {
        public static List<string> FromModelState(ModelStateDictionary modelState)
        {
            return modelState.Values
                             .SelectMany(v => v.Errors)
                             .Select(e => e.ErrorMessage)
                             .ToList();
        }
    }
}
