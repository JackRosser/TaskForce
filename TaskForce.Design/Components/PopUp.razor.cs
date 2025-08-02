using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
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
        private ElementReference ModalRef;

        // All'apertura il focus va sullo script
        public async Task Open()
        {
            IsVisible = true;
            await InvokeAsync(StateHasChanged); // forza il render
            await Task.Delay(1); // assicura DOM pronto
            if (IsForm && Model is not null)
            {
                await JS.InvokeVoidAsync("eval", @"
                (function(modal) {
                    if (!modal) return;
                    const input = modal.querySelector('input, textarea, select');
                    if (input && input.offsetParent !== null) {
                        input.focus();
                        if (typeof input.select === 'function') input.select();
                    }
                })(document.querySelector('.modal.show'));
            ");
            }
        }

        private async Task Submit()
        {
            await OnSubmit.InvokeAsync();
            await Update.InvokeAsync();
            Close();
        }

        private void Close() => IsVisible = false;

    }

}
