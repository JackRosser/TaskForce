using TaskForce.Client.Components.ElementiProgetti;
using TaskForce.Dto.Progetto;

namespace TaskForce.Client.Components
{
    public partial class Progetti : ProgettiBase
    {
        private List<HeaderText> Headers { get; set; } = new();
        private string? TempoMancante { get; set; }
        private int? GiorniLavorativi { get; set; }
        private int? ProgettoVisualizzatoId { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            await FirstOrLast();
        }

        private async Task FirstOrLast(bool last = false)
        {
            SetHeaders();

            if (List is null || !List.Any())
                return;

            if (last)
            {
                CambiaProgettoVisualizzato(List.Last());
            }
            else
            {
                CambiaProgettoVisualizzato(List.First());
            }

            await Task.CompletedTask;
        }

        private void SetHeaders()
        {
            Headers = new()
            {
                new() { Name = "giorni previsti" },
                new() { Name = "stato" },
                new() { Name = "lavorazione" }
            };
        }

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

        private class HeaderText
        {
            public string? Name { get; set; }
        }


        private void SetId(int id)
        {
            ProgettoVisualizzatoId = id;
        }

        private void CambiaProgettoVisualizzato(GetProgettoWithFasiRequest? progetto)
        {
            if (progetto is null)
                return;

            SetId(progetto.Id);
            SetTempoMancante(progetto.Consegna);
        }

        private async Task NuovoProgettoVisualizzatoImmediatamente()
        {
            await Update.InvokeAsync();
            await FirstOrLast(true);
        }

    }
}
