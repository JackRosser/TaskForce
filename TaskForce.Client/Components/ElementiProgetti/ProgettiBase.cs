using Microsoft.AspNetCore.Components;
using TaskForce.Client.Abstractions;
using TaskForce.Dto.Progetto;

namespace TaskForce.Client.Components.ElementiProgetti
{
    public abstract class ProgettiBase : SdkBase
    {
        [Parameter] public IEnumerable<GetProgettoWithFasiRequest>? List { get; set; }
        [Parameter] public EventCallback Update { get; set; }
    }
}
