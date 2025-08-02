using TaskForce.Client.Components.ElementiProgetti;
using TaskForce.Dto.Progetto;

namespace TaskForce.Client.Components
{
    public partial class Progetti : ProgettiBase
    {
        private List<HeaderText> Headers { get; set; } = new();
        private CountDown _countDown { get; set; } = new();
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
            _countDown.SetTempoMancante(progetto.Consegna);
        }

        private async Task NuovoProgettoVisualizzatoImmediatamente()
        {
            await UpdateParent();
            await FirstOrLast(true);
        }

    }
}
