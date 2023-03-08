namespace Confab.Modules.Attendances.Domain.Policies;

public interface ISlotPolicyFactory
{
    ISlotPolicy Create(params string[] tags);
}