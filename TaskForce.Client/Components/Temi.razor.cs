using Microsoft.AspNetCore.Components;
using TaskForce.Design.Abstractions;
using TaskForce.Design.Components;
using TaskForce.Enum;
using TaskForce.LocalStorage;

namespace TaskForce.Client.Components
{
    public partial class Temi(TemaStorageService Storage) : LayoutBase
    {
        [CascadingParameter(Name = "OnInit")] public EventCallback UpdateLayout { get; set; }

        private List<DropDownItem>? DropDownTemi { get; set; }
        private List<TemaDetails>? TemiDisponibili { get; set; }

        public Tema? TemaSelezionato => Theme;

        private async Task CambiaTemaAsync(Tema nuovoTema)
        {
            Theme = nuovoTema;
            await Storage.SetTemaAsync(nuovoTema);
            //await UpdateLayout.InvokeAsync();
            Navigation.NavigateTo(Navigation.Uri, forceLoad: true);
        }

        protected override void OnInitialized()
        {
            DropDownTemi = new();
            TemiDisponibili = new()
            {
                new() { Value = Tema.vs, Nome = "Visual Studio" },
                new() { Value = Tema.isys, Nome = "I-System" },
            };

            foreach (var tema in TemiDisponibili)
            {
                DropDownTemi.Add(new DropDownItem
                {
                    Text = tema.Nome,
                    Action = EventCallback.Factory.Create(this, () => CambiaTemaAsync(tema.Value))
                });
            }
        }

        private class TemaDetails
        {
            public Tema Value { get; set; }
            public string? Nome { get; set; }
        }
    }
}
