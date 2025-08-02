using Microsoft.AspNetCore.Components;
using TaskForce.Design.Abstractions;

namespace TaskForce.Design.Components
{
    public partial class Icon : LayoutBase
    {
        [Parameter, EditorRequired] public string? Element { get; set; }
        [Parameter] public bool Pointer { get; set; }
        [Parameter] public string? Color { get; set; }
        [Parameter] public string? ToolTip { get; set; }
        [Parameter] public string? CssClass { get; set; }
        [Parameter] public EventCallback OnClick { get; set; }

        protected override void OnParametersSet()
        {
            Color ??= $"{Theme}-primary";
        }
    }

    public static class Icons
    {
        public static string Gear = "bi bi-gear";
        public static string Folder = "bi bi-folder-plus";
        public static string Edit = "bi bi-pencil-fill";
        public static string AddUser = "bi bi-person-plus-fill";
    }
}
