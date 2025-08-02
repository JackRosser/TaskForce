using Microsoft.AspNetCore.Components;
using TaskForce.Design.Abstractions;
using TaskForce.Enum;

namespace TaskForce.Design.Components
{
    public partial class Badge : LayoutBase
    {
        /// <summary>
        /// Badge per un oggetto di tipo StatoFase.
        /// </summary>
        [Parameter] public StatoFase? StatoDiFase { get; set; }
        /// <summary>
        /// Badge per un oggetto di tipo StatoUtente.
        /// </summary>
        [Parameter] public StatoUtente? StatoDiUtente { get; set; }
        /// <summary>
        /// Testo in mancanza di un enum
        /// </summary>
        [Parameter] public string? Title { get; set; }
        /// <summary>
        /// Colore in mancanza di un enum
        /// </summary>
        [Parameter] public string? StaticColor { get; set; }
        private string? Text { get; set; }
        private string? Color { get; set; }


        protected override void OnParametersSet()
        {
            if (StatoDiFase is not null)
            {
                Text = StatoDiFase switch
                {
                    StatoFase.Completato => "Completato",
                    _ => "Da completare"
                };

                Color = StatoDiFase switch
                {
                    StatoFase.Completato => $"success",
                    _ => $"{Theme}-primary"
                };
            }

            if (StatoDiUtente is not null)
            {
                Text = StatoDiUtente switch
                {
                    StatoUtente.Attivo => "Attivo",
                    StatoUtente.Concluso => "Concluso",
                    _ => "Pausa"
                };

                Color = StatoDiUtente switch
                {
                    StatoUtente.Attivo => $"{Theme}-primary",
                    StatoUtente.Concluso => $"success",
                    _ => $"warning"
                };
            }

            if (!string.IsNullOrEmpty(Title)) Text = Title;
            if (!string.IsNullOrEmpty(StaticColor)) Color = StaticColor;
        }
    }
}
