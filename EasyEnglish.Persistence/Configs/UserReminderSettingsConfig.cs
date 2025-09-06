using EasyEnglish.Core.Persistence;
using EasyEnglish.Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace EasyEnglish.Persistence.Configs;

internal sealed class UserReminderSettingsConfig : EntityTypeConfigurationBase<UserReminderSettings>
{
    protected override void ConfigureMore(EntityTypeBuilder<UserReminderSettings> builder) { }
}