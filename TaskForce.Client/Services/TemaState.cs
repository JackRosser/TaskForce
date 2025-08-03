using TaskForce.Enum;

namespace TaskForce.Client.Services
{
    public class TemaState
    {
        public Tema Value { get; private set; }

        public event Action? OnChange;

        public void Set(Tema value)
        {
            Value = value;
            OnChange?.Invoke();
        }
    }


}
