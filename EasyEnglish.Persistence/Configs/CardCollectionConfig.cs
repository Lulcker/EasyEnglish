using EasyEnglish.Core.Persistence;
using EasyEnglish.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyEnglish.Persistence.Configs;

internal sealed class CardCollectionConfig : EntityTypeConfigurationBase<CardCollection>
{
    protected override void ConfigureMore(EntityTypeBuilder<CardCollection> builder)
    {
        builder.HasMany(p => p.Cards)
            .WithOne(g => g.CardCollection)
            .HasForeignKey(g => g.CardCollectionId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}