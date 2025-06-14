namespace TaskManagerAPI.Validate
{
    public static class DateHelper
    {
        public static string? ToDisplayDate(DateTime? date)
        {
            return date?.ToString("dd/MM/yyyy");
        }

        public static DateTime? ParseExactOrNull(string? dateString)
        {
            if (string.IsNullOrWhiteSpace(dateString))
                return null;

            if (DateTime.TryParseExact(dateString, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var result))
                return result;

            throw new FormatException("Invalid date format. Please use dd/MM/yyyy.");
        }
    }
}