using TaskForce.Client.Components.ElementiProgetti;
using TaskForce.Dto.Progetto;

namespace TaskForce.Client.Components
{
    public partial class DevManagement : ProgettiBase
    {
        private GetPresaInCaricoDettaglioDto? Lavorazione { get; set; }
        public async Task Start(GetPresaInCaricoDettaglioDto lavorazione)
        {
            Lavorazione = lavorazione;
            await StartFormOnPopUp();
        }
        private async Task Pausa()
        {
            if (Lavorazione is null) return;
            await Sdk.SendRequestAsync(r => r.PausaPresaInCaricoAsync(Lavorazione.Id));
            await UpdateParent();
            await _popup.Close();
        }

        private async Task RiprendiLavorazione()
        {
            if (Lavorazione is null) return;
            await Sdk.SendRequestAsync(r => r.RiprendiPresaInCaricoAsync(Lavorazione.Id));
            await UpdateParent();
            await _popup.Close();
        }

        private async Task FineLavorazione()
        {
            if (Lavorazione is null) return;
            await Sdk.SendRequestAsync(r => r.ConcludiPresaInCaricoAsync(Lavorazione.Id));
            await UpdateParent();
            await _popup.Close();
        }

        private async Task EliminaLavorazione()
        {
            if (Lavorazione is null) return;
            await Sdk.SendRequestAsync(r => r.EliminaPresaInCaricoAsync(Lavorazione.Id));
            await UpdateParent();
            await _popup.Close();
        }
    }
}
