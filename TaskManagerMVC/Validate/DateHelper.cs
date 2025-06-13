namespace TaskManagerAPI.Validate
{
    public static class DateHelper
    {
        public static DateTime? ParseExactOrNull(string? dateStr)
        {
            if (string.IsNullOrWhiteSpace(dateStr)) return null;
            if (DateTime.TryParseExact(dateStr, "dd/MM/yyyy", null, System.Globalization.DateTimeStyles.None, out var dt))
                return dt;
            throw new FormatException($"Ngày không hợp lệ: {dateStr}. Định dạng: dd/MM/yyyy.");
        }

        public static string? ToDisplayDate(DateTime? date)
        {
            return date?.ToString("dd/MM/yyyy");
        }
    }

}
