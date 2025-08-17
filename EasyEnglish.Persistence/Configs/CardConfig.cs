using EasyEnglish.Core.Persistence;
using EasyEnglish.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyEnglish.Persistence.Configs;

internal sealed class CardConfig : EntityTypeConfigurationBase<Card>
{
    protected override void ConfigureMore(EntityTypeBuilder<Card> builder) { }
}