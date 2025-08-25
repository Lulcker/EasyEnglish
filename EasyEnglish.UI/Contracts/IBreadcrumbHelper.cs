using MudBlazor;

namespace EasyEnglish.UI.Contracts;

public interface IBreadcrumbHelper
{
    event Action? OnChanged;
    
    public IReadOnlyList<BreadcrumbItem> Breadcrumbs { get; }
    
    public void SetBreadcrumbs(IEnumerable<BreadcrumbItem> breadcrumbs);
}