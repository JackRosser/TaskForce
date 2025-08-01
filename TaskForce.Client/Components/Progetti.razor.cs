using Microsoft.AspNetCore.Components;
using TaskForce.Design.Abstractions;
using TaskForce.Dto.Progetto;

namespace TaskForce.Client.Components
{
    public partial class Progetti : LayoutBase
    {
        [Parameter] public IEnumerable<GetProgettoWithFasiRequest>? List { get; set; }
        private List<HeaderText> Headers { get; set; } = new();
        private string? TempoMancante { get; set; }
        private int? GiorniLavorativi { get; set; }
        private int? ProgettoVisualizzatoId { get; set; }
        protected override void OnParametersSet()
        {
            SetHeaders();
            if (List is null)
            {
                return;
            }
            CambiaProgettoVisualizzato(List.FirstOrDefault());

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

        private void CambiaProgettoVisualizzato(GetProgettoWithFasiRequest progetto)
        {
            SetId(progetto.Id);
            SetTempoMancante(progetto.Consegna);
        }

    }
}
