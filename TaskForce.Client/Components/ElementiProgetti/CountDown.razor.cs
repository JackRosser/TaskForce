using Microsoft.AspNetCore.Components;
using TaskForce.Dto.Progetto;
using TaskForce.Dto.Progetto.FasiProgetto.MacroFasi;

namespace TaskForce.Client.Components.ElementiProgetti
{
    public partial class CountDown : ProgettiBase
    {
        [Parameter] public EventCallback<GetProgettoWithFasiRequest> SezioneCreata { get; set; }
        private CreateMacroFaseDto? Form { get; set; }
        protected override void OnParametersSet()
        {
            if (Progetto is null) return;
            SetTempoMancante(Progetto.Consegna);
        }

        private string? TempoMancante { get; set; }
        private int? GiorniLavorativi { get; set; }

        public void SetTempoMancante(DateTime target)
        {
            var oggi = DateTime.Today;

            var totalDays = (target - oggi).Days;

            // Arrotonda sempre per eccesso
            var mesi = (int)Math.Floor(totalDays / 30.0);
            var giorniResidui = (int)Math.Ceiling(totalDays - mesi * 30.0);

            TempoMancante = $"{mesi} mesi {giorniResidui} giorni";

            int lavorativi = 0;
            for (var d = oggi; d < target; d = d.AddDays(1))
            {
                if (d.DayOfWeek != DayOfWeek.Saturday && d.DayOfWeek != DayOfWeek.Sunday)
                {
                    lavorativi++;
                }
            }

            GiorniLavorativi = lavorativi;
        }


        private async Task Submit()
        {
            if (Progetto is null) return;
            await Sdk.SendRequestAsync(r => r.CreateMacroFaseAsync(Progetto.Id, Form));

        }

        public async Task Start()
        {
            Form = new();
            await StartFormOnPopUp();
        }

        private async Task AfterCreated()
        {
            if (Progetto is null) return;
            if (SezioneCreata.HasDelegate) { await SezioneCreata.InvokeAsync(Progetto); }
        }
    }
}
