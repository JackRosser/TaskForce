using TaskForce.Client.Abstractions;
using TaskForce.Client.Pages;
using TaskForce.Dto.Progetto;

namespace TaskForce.Client.Components
{
    public partial class NuovoProgetto : SdkBase
    {

        private Home _home { get; set; } = null!;
        private CreaProgettoDto? Form { get; set; }
        private async Task Submit()
        {
            await Sdk.SendRequestAsync(r => r.CreaProgettoAsync(Form));
        }

        public async Task Start()
        {
            Form = new();
            await StartFormOnPopUp();
        }


        protected override async Task GetRecords()
        {
            await _home.Refresh();
        }
    }
}
