using Microsoft.EntityFrameworkCore;

namespace Application.Services.TripTranslations
{
    internal static class TripTranslationUniqueConstraint
    {
        public static bool IsViolation(DbUpdateException exception)
        {
            var message = exception.InnerException?.Message ?? exception.Message;
            return message.Contains("IX_TripTranslations_TripId_Language", StringComparison.OrdinalIgnoreCase)
                || message.Contains("unique index", StringComparison.OrdinalIgnoreCase);
        }
    }
}
