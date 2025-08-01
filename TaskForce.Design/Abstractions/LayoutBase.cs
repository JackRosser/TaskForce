using Microsoft.AspNetCore.Components;

namespace TaskForce.Design.Abstractions
{
    public abstract class LayoutBase : TaskForceBase
    {
        [CascadingParameter(Name = "Theme")] public string? Theme { get; set; }
        protected bool IsLoading { get; set; } = true;
    }
}
