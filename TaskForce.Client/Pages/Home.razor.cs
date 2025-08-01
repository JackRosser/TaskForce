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
        }

        protected override async Task GetRecords()
        {
            var response = await Sdk.SendRequestAsync(c => c.GetProgettiWithFasiInfoAsync());
            if (response.IsFailed) return;
            Progetti = response.Value;
        }

        // HEADER
        private List<HeaderText>? Headers { get; set; }
        private void SetHeaders()
        {
            Headers = new()
            {
                new() { Name = "fase" },
                new() { Name = "backend" },
                new() { Name = "ui/ux" },
                new() { Name = "stato" },
                new() { Name = "presa in carico" }
            };
        }
        private class HeaderText
        {
            public string? Name { get; set; }
        }
    }
}
