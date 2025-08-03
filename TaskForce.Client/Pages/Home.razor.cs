using TaskForce.Client.Abstractions;
using TaskForce.Client.Components;
using TaskForce.Dto.Progetto;

namespace TaskForce.Client.Pages
{
    public partial class Home : SdkBase
    {
        private IEnumerable<GetProgettoWithFasiRequest>? Progetti { get; set; }
        private NuovoProgetto _nuovoProgetto { get; set; } = new();
        private bool NessunProgettoInizializzato { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await GetRecords();
        }

        protected override async Task GetRecords()
        {
            IsLoading = true;
            var response = await Sdk.SendRequestAsync(c => c.GetProgettiWithFasiInfoAsync());
            if (response.IsFailed) return;
            Progetti = response.Value;
            if (Progetti is null || !Progetti.Any()) NessunProgettoInizializzato = true;
            IsLoading = false;
        }

    }
}
