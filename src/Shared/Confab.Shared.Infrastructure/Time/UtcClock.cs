using Confab.Shared.Abstractions.Time;

namespace Confab.Shared.Infrastructure.Time;

public class UtcClock : IClock
{
    public DateTime CurrentDate()
    {
        return DateTime.UtcNow;
    }
}