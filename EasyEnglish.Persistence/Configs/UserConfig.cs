using EasyEnglish.Core.Persistence;
using EasyEnglish.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyEnglish.Persistence.Configs;

internal sealed class UserConfig : EntityTypeConfigurationBase<User>
{
    protected override void ConfigureMore(EntityTypeBuilder<User> builder)
    {
        builder.HasIndex(u => u.Email).IsUnique();

        builder.HasMany(u => u.CardCollections)
            .WithOne(c => c.User)
            .HasForeignKey(c => c.UserId)
            .OnDelete(DeleteBehavior.Restrict);
        
        builder.HasOne(u => u.ReminderSettings)
            .WithOne(r => r.User)
            .HasForeignKey<UserReminderSettings>(r => r.UserId)
            .OnDelete(DeleteBehavior.Restrict);
    }
}