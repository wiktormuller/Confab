﻿using Confab.Modules.Agendas.Domain.CallForPapers.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using Confab.Shared.Abstractions.Kernel.Types;

namespace Confab.Modules.Agendas.Infrastructure.EF.Configurations;

internal class CallForPapersConfiguration : IEntityTypeConfiguration<CallForPapers>
{
    public void Configure(EntityTypeBuilder<CallForPapers> builder)
    {
        builder.HasKey(s => s.Id);

        builder
            .Property(s => s.Id)
            .HasConversion(id => id.Value, id => new AggregateId(id));

        builder
            .Property(s => s.ConferenceId)
            .HasConversion(id => id.Value, id => new ConferenceId(id));

        builder
            .Property(cfp => cfp.Version)
            .IsConcurrencyToken();
    }
}