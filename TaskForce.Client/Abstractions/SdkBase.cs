using Microsoft.AspNetCore.Components;
using TaskForce.Client.Services;
using TaskForce.Design.Abstractions;
using TaskForce.Design.Components;

namespace TaskForce.Client.Abstractions
{
    public abstract class SdkBase : LayoutBase
    {
        [Inject] protected SdkService Sdk { get; set; } = default!;
        protected virtual Task GetRecords()
        {
            return Task.CompletedTask;
        }
        protected PopUp _popup { get; set; } = null!;

        protected async Task StartFormOnPopUp()
        {

            StateHasChanged(); // forzi il re-render per propagare il parametro al PopUp
            await Task.Yield(); // permette a Blazor di terminare il ciclo di rendering
            await _popup.Open(); // ora Model è sicuramente assegnato
        }

    }
}
