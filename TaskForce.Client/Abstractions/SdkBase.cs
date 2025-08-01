using Microsoft.AspNetCore.Components;
using TaskForce.Client.Services;
using TaskForce.Design.Abstractions;

namespace TaskForce.Client.Abstractions
{
    public abstract class SdkBase : LayoutBase
    {
        [Inject] protected SdkService Sdk { get; set; } = default!;
    }
}
