namespace Confab.Modules.Attendances.Domain.Policies;

public class SlotPolicyFactory : ISlotPolicyFactory
{
    public ISlotPolicy Create(params string[] tags)
    {
        return tags switch
        {
            { } when tags.Contains("stationary") => new RegularSlotPolicy(),
            { } when tags.Contains("workshop") => new RegularSlotPolicy(),
            _ => new OverbookingSlotPolicy()
        };
    }
}
