using Hangfire.Dashboard;

namespace EasyEnglish.Filters;

internal class AllowAllDashboardAuthorizationFilter : IDashboardAuthorizationFilter
{
    public bool Authorize(DashboardContext context) => true;
}