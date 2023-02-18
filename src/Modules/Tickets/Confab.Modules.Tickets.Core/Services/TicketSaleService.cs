using Confab.Modules.Tickets.Core.DTO;

namespace Confab.Modules.Tickets.Core.Services;

internal class TicketSaleService : ITicketSaleService
{
    public TicketSaleService()
    {
        
    }
    
    public Task AddAsync(TicketSaleDto dto)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<TicketSaleInfoDto>> GetAllAsync(Guid conferenceId)
    {
        throw new NotImplementedException();
    }

    public Task<TicketSaleInfoDto> GetCurrentAsync(Guid conferenceId)
    {
        throw new NotImplementedException();
    }
}