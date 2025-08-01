using DevExpress.Blazor;

namespace TaskForce.Client.EventAggregator
{
    public class StateContainerService()
    {

        /// <summary>
        /// The event that will be raised for state changed
        /// </summary>
        public event Action? OnMessageAdded;
        public event Action<ToastOptions>? OnToastMessageAdded;
        public event Action? OnAutoSize;

        public void Notify(Message msg)
        {
            NotifyMessageAdded();
        }

        public void NotifyToast(ToastOptions options)
        {
            OnToastMessageAdded?.Invoke(options);
        }
        public void NotifyAutoSize()
        {
            OnAutoSize?.Invoke();
        }

        private void NotifyMessageAdded() => OnMessageAdded?.Invoke();

    }
}
