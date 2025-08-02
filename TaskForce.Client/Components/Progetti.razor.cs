using TaskForce.Client.Components.ElementiProgetti;
using TaskForce.Dto.Progetto;
using TaskForce.Dto.Progetto.FasiProgetto;

namespace TaskForce.Client.Components
{
    public partial class Progetti : ProgettiBase
    {
        private List<HeaderText> Headers { get; set; } = new();
        private CountDown _countDown { get; set; } = new();
        private int? ProgettoVisualizzatoId { get; set; }
        private int? MacroFaseSelezionataId { get; set; }
        private GetProgettoWithFasiRequest? ProgettoSelezionato { get; set; }
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

        private async Task AfterNuovaSezione(GetProgettoWithFasiRequest proj)
        {
            await UpdateParent();
            CambiaProgettoVisualizzato(proj);

        }


        // Nuova fase

        private CreateFaseDto? Form { get; set; }

        public async Task Start(GetProgettoWithFasiRequest proj, int id)
        {
            ProgettoSelezionato = proj;
            MacroFaseSelezionataId = id;
            Form = new();
            await StartFormOnPopUp();
        }

        private async Task Submit()
        {
            await Sdk.SendRequestAsync(r => r.CreateFaseAsync(MacroFaseSelezionataId, Form));
        }

        private async Task AfterNuovaFase()
        {
            await UpdateParent();
            CambiaProgettoVisualizzato(ProgettoSelezionato);

        }



    }
}
