using TaskForce.Client.Abstractions;
using TaskForce.Dto;

namespace TaskForce.Client.Pages
{
    public partial class Globals : SdkBase
    {
        private IEnumerable<PortfolioProjectOverviewDto>? Progetti { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await GetRecords();
        }
        protected override async Task GetRecords()
        {
            IsLoading = true;
            var result = await Sdk.SendProgettiRequestAsync(r => r.GetProgettiPortfolioOverviewAsync());
            if (!result.IsSuccess) return;
            Progetti = result.Value;
            IsLoading = false;
        }
    }
}
