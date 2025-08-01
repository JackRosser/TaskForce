using TaskForce.Client.Services;
using TaskForce.Design.Abstractions;
using TaskForce.Dto.User;

namespace TaskForce.Client.Pages
{
    public partial class Home(SdkService Sdk) : LayoutBase
    {
        private IEnumerable<GetUserDto>? Users { get; set; }

        protected override async Task OnInitializedAsync()
        {
            await GetRecords();
        }

        protected override async Task GetRecords()
        {
            var response = await Sdk.SendRequestAsync(c => c.GetAllUsersAsync());
            if (response.IsFailed) return;
            Users = response.Value;
        }
    }
}
