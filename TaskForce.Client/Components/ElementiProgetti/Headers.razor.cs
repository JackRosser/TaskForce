using Microsoft.AspNetCore.Components;

namespace TaskForce.Client.Components.ElementiProgetti
{
    public partial class Headers : ProgettiBase
    {

        [Parameter] public int? ProgettoAttivo { get; set; }
        [Parameter] public EventCallback<int> ProgettoAttivoChanged { get; set; }
        private NuovoProgetto _nuovoProgetto { get; set; } = null!;
        private async Task Change(int id)
        {
            await ProgettoAttivoChanged.InvokeAsync(id);
        }
        private async Task NuovoProgettoCreato()
        {
            await Update.InvokeAsync();
        }

    }
}
