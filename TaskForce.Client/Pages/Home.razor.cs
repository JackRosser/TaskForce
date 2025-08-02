using TaskForce.Client.Abstractions;
using TaskForce.Dto.Progetto;

namespace TaskForce.Client.Pages
{
    public partial class Home : SdkBase
    {
        private IEnumerable<GetProgettoWithFasiRequest>? Progetti { get; set; }
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
            IsLoading = false;
        }

        public async Task Refresh()
        {
            await GetRecords();
        }


    }
}
