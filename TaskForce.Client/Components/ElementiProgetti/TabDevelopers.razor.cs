using Microsoft.AspNetCore.Components;
using TaskForce.Dto.Progetto;
using TaskForce.Dto.Progetto.PresaInCarico;
using TaskForce.Dto.User;

namespace TaskForce.Client.Components.ElementiProgetti
{
    public partial class TabDevelopers : ProgettiBase
    {
        [Parameter] public GetFaseDettaglioDto? Fase { get; set; }
        private IEnumerable<GetUserDto>? Users { get; set; }
        private PresaInCaricoRequest? Form { get; set; }
        private async Task Submit()
        {
            if (Fase is null) return;
            await Sdk.SendRequestAsync(r => r.InitPresaInCaricoAsync(Fase.Id, Form));
        }

        public async Task Start()
        {
            if (Fase is null) return;
            Form = new();
            var response = await Sdk.SendRequestAsync(u => u.GetUtentiNonAssegnatiAllaFaseAsync(Fase.Id));
            if (response.IsFailed) { return; }
            Users = response.Value;
            await StartFormOnPopUp();
        }

    }
}
