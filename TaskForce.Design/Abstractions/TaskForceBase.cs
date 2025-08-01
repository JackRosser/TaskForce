using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;

namespace TaskForce.Design.Abstractions
{
    public abstract class TaskForceBase : ComponentBase, IDisposable
    {
        [Inject] protected IJSRuntime JS { get; set; } = default!;
        [Inject] protected NavigationManager Navigation { get; set; } = default!;

        protected CancellationTokenSource Ct = new();
        protected virtual void OnDispose() { }

        public void Dispose()
        {
            Ct.Cancel();
            Ct.Dispose();
            OnDispose();
        }

    }
}
