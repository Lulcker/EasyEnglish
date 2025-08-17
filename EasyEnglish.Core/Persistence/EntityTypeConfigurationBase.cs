using EasyEnglish.Core.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyEnglish.Core.Persistence;

public abstract class EntityTypeConfigurationBase<TEntity> : IEntityTypeConfiguration<TEntity> where TEntity: EntityBase
{
    public void Configure(EntityTypeBuilder<TEntity> builder)
    {
        builder.HasKey(e => e.Id);
        builder.Property(e => e.Id).ValueGeneratedNever();
        
        ConfigureMore(builder); 
    }

    protected abstract void ConfigureMore(EntityTypeBuilder<TEntity> builder);
}