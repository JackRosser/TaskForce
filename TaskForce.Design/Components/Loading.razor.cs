using Microsoft.AspNetCore.Components;
using TaskForce.Design.Abstractions;

namespace TaskForce.Design.Components
{
    public partial class Loading : LayoutBase
    {
        [Parameter] public bool Toggle { get; set; }
    }
}
