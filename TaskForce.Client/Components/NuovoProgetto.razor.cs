using Microsoft.AspNetCore.Components;
using TaskForce.Client.Abstractions;
using TaskForce.Dto.Progetto;

namespace TaskForce.Client.Components
{
    public partial class NuovoProgetto : SdkBase
    {
        [Parameter] public EventCallback Update { get; set; }
        private CreaProgettoDto? Form { get; set; }
        private async Task Submit()
        {
            await Sdk.SendProgettiRequestAsync(r => r.CreaProgettoAsync(Form));

        }

        public async Task Start()
        {
            Form = new();
            await StartFormOnPopUp();
        }

        private async Task OnUpdate()
        {
            if (Update.HasDelegate)
            {
                await Update.InvokeAsync();
            }
        }

    }
}
