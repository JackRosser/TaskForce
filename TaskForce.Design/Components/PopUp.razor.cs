using Microsoft.AspNetCore.Components;
using TaskForce.Design.Abstractions;

namespace TaskForce.Design.Components
{
    public partial class PopUp : LayoutBase
    {
        [Parameter] public RenderFragment? ChildContent { get; set; }
        [Parameter] public string? Title { get; set; }
        [Parameter] public EventCallback OnSubmit { get; set; }
        [Parameter] public EventCallback Update { get; set; }
        [Parameter] public bool IsForm { get; set; }
        [Parameter] public object? Model { get; set; }
        private bool IsVisible { get; set; }
        public async Task Open()
        {
            IsVisible = true;
            await InvokeAsync(StateHasChanged);
        }
        private async Task Submit()
        {
            await OnSubmit.InvokeAsync();
            await Update.InvokeAsync();
            Close();
        }
        private void Close()
        {
            IsVisible = false;
        }

    }

}
