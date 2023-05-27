using Microsoft.Extensions.DependencyInjection;

namespace EDeals.Core.Infrastructure.Shared.DateTimeHelpers
{
    public interface IDateTimeHelper
    {
        DateTime GetUtcNow();
        DateTime GetNow();

        DateTimeOffset GetDateTimeOffsetUtcNow();
        DateTimeOffset GetDateTimeOffsetNow();
    }

    public class DateTimeHelper : IDateTimeHelper
    {
        public DateTimeOffset GetDateTimeOffsetNow()
        {
            return DateTimeOffset.Now;
        }

        public DateTimeOffset GetDateTimeOffsetUtcNow()
        {
            return DateTimeOffset.UtcNow;
        }

        public DateTime GetNow()
        {
            return DateTime.Now;
        }

        public DateTime GetUtcNow()
        {
            return DateTime.UtcNow;
        }
    }
}
