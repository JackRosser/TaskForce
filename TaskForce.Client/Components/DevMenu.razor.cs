using Microsoft.AspNetCore.Components;
using TaskForce.Client.Abstractions;
using TaskForce.Design.Components;
using TaskForce.Dto.User;

namespace TaskForce.Client.Components
{
    public partial class DevMenu : SdkBase
    {
        private IEnumerable<GetUserDto>? Users { get; set; }
        private List<DropDownItem>? DropDownUsers { get; set; }
        private CreateUserDto? Form { get; set; }
        protected override async Task OnInitializedAsync()
        {
            await GetRecords();
        }

        protected override async Task GetRecords()
        {
            var response = await Sdk.SendRequestAsync(u => u.GetAllUsersAsync());
            if (response.IsFailed) { return; }
            Users = response.Value;
            SetUsers();
        }
        private void SetUsers()
        {
            DropDownUsers = new()
            {
                new DropDownItem
                {
                    Text = "Aggiungi developer",
                    Action = EventCallback.Factory.Create(this, () => Start())
                }
            };

            if (Users is null) return;

            DropDownUsers.AddRange(
                Users.Select(u => new DropDownItem
                {
                    Text = u.Nome,
                    Action = EventCallback.Factory.Create(this, () => Start())
                })
            );
        }


        private async Task Submit()
        {
            await Sdk.SendRequestAsync(r => r.CreateNewUserAsync(Form));
            await GetRecords();

        }

        public async Task Start()
        {
            Form = new();
            await StartFormOnPopUp();
        }

    }
}
