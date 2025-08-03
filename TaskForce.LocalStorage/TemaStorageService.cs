using Blazored.LocalStorage;
using TaskForce.Enum;

namespace TaskForce.LocalStorage
{
    public class TemaStorageService
    {
        private const string Key = "TemaImpostato";
        private readonly ILocalStorageService _localStorage;

        public TemaStorageService(ILocalStorageService localStorage)
        {
            _localStorage = localStorage;
        }

        public async Task<Tema> GetTemaAsync()
        {
            var hasKey = await _localStorage.ContainKeyAsync(Key);
            if (!hasKey)
            {
                await SetTemaAsync(Tema.vs);
                return Tema.vs;
            }

            return await _localStorage.GetItemAsync<Tema>(Key);
        }

        public async Task SetTemaAsync(Tema tema)
        {
            await _localStorage.SetItemAsync(Key, tema);
        }
    }
}
