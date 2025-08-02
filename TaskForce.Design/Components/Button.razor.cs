using Microsoft.AspNetCore.Components;
using TaskForce.Design.Abstractions;

namespace TaskForce.Design.Components
{
    public partial class Button : LayoutBase
    {
        [Parameter] public string? Text { get; set; }
        [Parameter] public EventCallback OnClick { get; set; }
        [Parameter] public string? Color { get; set; }
        [Parameter] public bool IsSubmit { get; set; }
        [Parameter] public string? CssClass { get; set; }
    }
}
