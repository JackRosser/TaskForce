using Microsoft.AspNetCore.Components;
using TaskForce.Design.Abstractions;
using TaskForce.Design.Components;

namespace TaskForce.Client.Components
{
    public partial class SezioneProgetti : LayoutBase
    {
        [CascadingParameter(Name = "OnInit")] public EventCallback UpdateLayout { get; set; }

        private List<DropDownItem>? DropDownProgetti { get; set; }


        protected override void OnInitialized()
        {
            DropDownProgetti = new()
            {
                new() { Text = "Visione globale", Action = EventCallback.Factory.Create(this, () => NavManager.NavigateTo("/globals"))},
                new() { Text = "Singoli", Action = EventCallback.Factory.Create(this, () => NavManager.NavigateTo("/home"))}
            };

        }

    }
}
