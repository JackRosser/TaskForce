using Microsoft.AspNetCore.Components;
using TaskForce.Client.Abstractions;
using TaskForce.Design.Components;
using TaskForce.Dto.User;

namespace TaskForce.Client.Layout
{
    public partial class NavBar : SdkBase
    {
        private IEnumerable<GetUserDto>? Users { get; set; }
        private List<DropDownItem>? DropDownUsers { get; set; }
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
                    Action = EventCallback.Factory.Create(this, () => UserMenu("Nessuno"))
                }
            };

            if (Users is null) return;

            DropDownUsers.AddRange(
                Users.Select(u => new DropDownItem
                {
                    Text = u.Nome,
                    Action = EventCallback.Factory.Create(this, () => UserMenu(u.Nome))
                })
            );
        }

        private void UserMenu(string nome)
        {
            Console.WriteLine($"User menu clicked for: {nome}");
        }

    }
}
