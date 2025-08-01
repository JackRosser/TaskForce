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
                SetId(Progetti.FirstOrDefault().Id);
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
    }
}
