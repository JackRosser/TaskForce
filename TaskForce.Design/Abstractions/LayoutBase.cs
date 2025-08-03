using Microsoft.AspNetCore.Components;
using TaskForce.Enum;

namespace TaskForce.Design.Abstractions
{
    public abstract class LayoutBase : TaskForceBase
    {
        [CascadingParameter(Name = "Theme")] public Tema? Theme { get; set; }
        protected bool IsLoading { get; set; } = true;
    }
}
