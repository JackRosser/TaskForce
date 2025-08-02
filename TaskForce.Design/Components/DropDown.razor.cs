using Microsoft.AspNetCore.Components;
using TaskForce.Design.Abstractions;

namespace TaskForce.Design.Components
{
    public partial class DropDown : LayoutBase
    {
        [Parameter] public string Color { get; set; } = "active";
        [Parameter] public bool FirstLineColor { get; set; }
        [Parameter, EditorRequired] public string? Text { get; set; }

        [Parameter, EditorRequired] public List<DropDownItem>? DropDownItems { get; set; }

    }

    public class DropDownItem
    {
        public string? Text { get; set; }
        public EventCallback Action { get; set; }
    }
}
