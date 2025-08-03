using TaskForce.Client.Services;
using TaskForce.Enum;
using TaskForce.LocalStorage;

namespace TaskForce.Client.Layout
{
    public partial class MainLayout(TemaStorageService Storage, TemaState TemaState)
    {
        protected Tema? Theme { get; set; }

        public async Task OnInit()
        {
            var tema = await Storage.GetTemaAsync();
            if (Theme != tema)
            {
                Theme = tema;
                StateHasChanged();
            }
            TemaState.Set(tema); // opzionale: se lo usi anche altrove
        }

        protected override async Task OnInitializedAsync()
        {
            await OnInit();
        }
    }







}
