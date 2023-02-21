using Confab.Shared.Abstractions.Time;

namespace Confab.Shared.Infrastructure.Time;

internal class UtcClock : IClock
{
    public DateTime CurrentDate()
    {
        return DateTime.UtcNow;
    }
}