using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Confab.Services.Tickets.Core.Entities;

namespace Confab.Services.Tickets.Core.DAL.Configurations;

internal class TicketConfiguration : IEntityTypeConfiguration<Ticket>
{
    public void Configure(EntityTypeBuilder<Ticket> builder)
    {
        builder.HasIndex(x => x.Code).IsUnique();
        builder.Property(x => x.UserId).IsConcurrencyToken();
    }
}
