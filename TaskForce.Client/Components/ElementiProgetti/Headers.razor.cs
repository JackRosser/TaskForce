using Microsoft.AspNetCore.Components;
using TaskForce.Design.Abstractions;
using TaskForce.Dto.Progetto;

namespace TaskForce.Client.Components.ElementiProgetti
{
    public partial class Headers : LayoutBase
    {
        [Parameter] public IEnumerable<GetProgettoWithFasiRequest>? List { get; set; }
        [Parameter] public int? ProgettoAttivo { get; set; }
        [Parameter] public EventCallback<int> ProgettoAttivoChanged { get; set; }
        private async Task Change(int id)
        {
            await ProgettoAttivoChanged.InvokeAsync(id);
        }

    }
}
