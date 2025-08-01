using TaskForce.Client.Abstractions;
using TaskForce.Dto.Progetto;

namespace TaskForce.Client.Pages
{
    public partial class Home : SdkBase
    {
        private IEnumerable<GetProgettoWithFasiRequest>? Progetti { get; set; }
        protected override async Task OnInitializedAsync()
        {
            SetHeaders();
            await GetRecords();
            if (Progetti is not null)
            {
                CambiaProgettoVisualizzato(Progetti.FirstOrDefault());
            }
        }

        protected override async Task GetRecords()
        {
            var response = await Sdk.SendRequestAsync(c => c.GetProgettiWithFasiInfoAsync());
            if (response.IsFailed) return;
            Progetti = response.Value;
        }

        // HEADER
        private List<HeaderText> Headers { get; set; } = new();
        private void SetHeaders()
        {
            Headers = new()
            {
                new() { Name = "fase" },
                new() { Name = "giorni previsti" },
                new() { Name = "stato" },
                new() { Name = "lavorazione" }
            };
        }
        private class HeaderText
        {
            public string? Name { get; set; }
        }

        // VISUALIZZAZIONE SINGOLI PROGETTI
        private int? ProgettoVisualizzatoId { get; set; }
        private void SetId(int id)
        {
            ProgettoVisualizzatoId = id;
        }

        private void CambiaProgettoVisualizzato(GetProgettoWithFasiRequest progetto)
        {
            SetId(progetto.Id);
            SetTempoMancante(progetto.Consegna);
        }

        // Countdown


        private string? TempoMancante { get; set; }
        private int? GiorniLavorativi { get; set; }

        private void SetTempoMancante(DateTime target)
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

    }
}
