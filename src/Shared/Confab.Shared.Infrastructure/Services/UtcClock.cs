using Confab.Shared.Abstractions;

namespace Confab.Shared.Infrastructure.Services;

internal class UtcClock : IClock
{
    public DateTime CurrentDate()
    {
        return DateTime.UtcNow;
    }
}