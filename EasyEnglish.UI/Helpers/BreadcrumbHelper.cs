using EasyEnglish.UI.Contracts;
using MudBlazor;

namespace EasyEnglish.UI.Helpers;

public class BreadcrumbHelper : IBreadcrumbHelper
{
    #region Private fields
    
    private List<BreadcrumbItem> breadcrumbs = [];
    
    #endregion
    
    public event Action? OnChanged;
    
    public IReadOnlyList<BreadcrumbItem> Breadcrumbs => breadcrumbs;
    
    public void SetBreadcrumbs(IEnumerable<BreadcrumbItem> newBreadcrumbs)
    {
        breadcrumbs = newBreadcrumbs.ToList();
        OnChanged?.Invoke();
    }
}