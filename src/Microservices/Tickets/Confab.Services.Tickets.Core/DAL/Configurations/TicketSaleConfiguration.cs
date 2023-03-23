using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Confab.Services.Tickets.Core.Entities;

namespace Confab.Services.Tickets.Core.DAL.Configurations;

internal class TicketSaleConfiguration : IEntityTypeConfiguration<TicketSale>
{
    public void Configure(EntityTypeBuilder<TicketSale> builder)
    {
    }
}
