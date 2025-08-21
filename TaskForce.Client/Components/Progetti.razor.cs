using Microsoft.AspNetCore.Components.Web;
using TaskForce.Client.Components.ElementiProgetti;
using TaskForce.Design.Components;
using TaskForce.Dto.Progetto;
using TaskForce.Dto.Progetto.FasiProgetto;
using TaskForce.Dto.Progetto.FasiProgetto.MacroFasi;

namespace TaskForce.Client.Components
{
    public partial class Progetti : ProgettiBase
    {
        private List<HeaderText> Headers { get; set; } = new();
        private CountDown _countDown { get; set; } = new();
        private int? ProgettoVisualizzatoId { get; set; }
        private int? MacroFaseSelezionataId { get; set; }
        private int? LavorazioneSelezionataId { get; set; }
        private GetProgettoWithFasiRequest? ProgettoSelezionato { get; set; }
        protected override async Task OnParametersSetAsync()
        {
            await FirstOrLast();
        }


        private async Task FirstOrLast(bool last = false)
        {
            SetHeaders();

            if (List is null || !List.Any())
                return;

            if (last)
            {
                CambiaProgettoVisualizzato(List.Last());
            }
            else
            {
                CambiaProgettoVisualizzato(List.First());
            }

            await Task.CompletedTask;
        }

        private void SetHeaders()
        {
            Headers = new()
            {
                new() { Name = "giorni previsti" },
                new() { Name = "stato" },
                new() { Name = "" }
            };
        }



        private class HeaderText
        {
            public string? Name { get; set; }
        }


        private void SetId(int id)
        {
            ProgettoVisualizzatoId = id;
        }

        private void CambiaProgettoVisualizzato(GetProgettoWithFasiRequest? progetto)
        {
            if (progetto is null)
                return;

            SetId(progetto.Id);
            _countDown.SetTempoMancante(progetto.Consegna);
        }

        private async Task NuovoProgettoVisualizzatoImmediatamente()
        {
            await UpdateParent();
            await FirstOrLast(true);
        }

        private async Task AfterNuovaSezione(GetProgettoWithFasiRequest proj)
        {
            await UpdateParent();
            CambiaProgettoVisualizzato(proj);

        }


        // Nuova fase

        private CreateFaseDto? Form { get; set; }

        private async Task Start(GetProgettoWithFasiRequest proj, int id)
        {
            ProgettoSelezionato = proj;
            MacroFaseSelezionataId = id;
            Form = new();
            await StartFormOnPopUp();
        }

        private async Task Submit()
        {
            await Sdk.SendRequestAsync(r => r.CreateFaseAsync(MacroFaseSelezionataId, Form));
        }

        private async Task AfterNuovaFase()
        {
            await UpdateParent();
            CambiaProgettoVisualizzato(ProgettoSelezionato);

        }

        // Edit Macro Fase
        private UpdateMacroFaseDto? UpdateForm { get; set; }
        private PopUp _popupEditSection { get; set; } = null!;
        private async Task ModificaSezione()
        {
            await Sdk.SendRequestAsync(r => r.UpdateMacroFaseAsync(MacroFaseSelezionataId.Value, UpdateForm));
        }
        private async Task StartEdit(GetMacroFaseDettaglioDto macro)
        {
            MacroFaseSelezionataId = macro.Id;
            UpdateForm = new()
            {
                Nome = macro.Nome,
            };
            StateHasChanged();
            await Task.Yield();
            await _popupEditSection.Open();
        }

        private async Task EliminaSezione()
        {
            var response = await Sdk.SendRequestAsync(r => r.DeleteMacroFaseAsync(MacroFaseSelezionataId.Value));
            if (response.IsSuccess)
            {
                await _popupEditSection.Close();
                await AfterNuovaFase();
            }
        }

        // Edit Lavorazione
        private UpdateFaseProgettoDto? UpdateFaseForm { get; set; }
        private PopUp _popupEditFase { get; set; } = null!;
        private async Task ModificaFase()
        {
            if (LavorazioneSelezionataId is null) return;
            await Sdk.SendRequestAsync(r => r.UpdateFaseAsync(LavorazioneSelezionataId.Value, UpdateFaseForm));
        }
        private async Task StartEditFase(GetFaseDettaglioDto lavorazione)
        {
            LavorazioneSelezionataId = lavorazione.Id;
            UpdateFaseForm = new()
            {
                Nome = lavorazione.Nome,
                GiorniPrevisti = lavorazione.GiorniPrevisti,
                TipoFase = lavorazione.TipoFase,
                Stato = lavorazione.Stato
            };
            StateHasChanged();
            await Task.Yield();
            await _popupEditFase.Open();
        }

        private async Task EliminaFase()
        {
            if (LavorazioneSelezionataId is null) return;
            var response = await Sdk.SendRequestAsync(r => r.DeleteFaseAsync(LavorazioneSelezionataId.Value));
            await _popupEditFase.Close();
            await AfterNuovaFase();
        }

        // DRAG E DROP

        private GetMacroFaseDettaglioDto? draggedMacro;

        private void OnDragStart(GetMacroFaseDettaglioDto macro)
        {
            draggedMacro = macro;
        }

        private async Task OnDrop(GetProgettoWithFasiRequest progetto, GetMacroFaseDettaglioDto targetMacro)
        {
            Console.WriteLine("OnDrop triggered");

            if (draggedMacro is null)
            {
                Console.WriteLine("draggedMacro è null");
                return;
            }

            if (draggedMacro.Id == targetMacro.Id)
            {
                Console.WriteLine("Trascinato su se stesso");
                return;
            }

            var lista = progetto.MacroFasi?.ToList();
            if (lista is null)
            {
                Console.WriteLine("MacroFasi è null");
                return;
            }

            int fromIndex = lista.FindIndex(m => m.Id == draggedMacro.Id);
            int toIndex = lista.FindIndex(m => m.Id == targetMacro.Id);

            if (fromIndex == -1 || toIndex == -1)
            {
                Console.WriteLine("Indici non trovati");
                return;
            }

            lista.RemoveAt(fromIndex);
            lista.Insert(toIndex, draggedMacro);

            for (int i = 0; i < lista.Count; i++)
                lista[i].Ordine = i + 1;

            progetto.MacroFasi = lista;
            ProgettoSelezionato = progetto;

            Console.WriteLine("Aggiorno server");
            await AggiornaOrdineServer(lista);
            draggedMacro = null;
        }

        private Task OnDragOver(DragEventArgs e)
        {
            Console.WriteLine("DragOver");
            return Task.CompletedTask;
        }


        private async Task AggiornaOrdineServer(IEnumerable<GetMacroFaseDettaglioDto> macroFasi)
        {
            if (ProgettoSelezionato is null) return;
            var aggiornamenti = macroFasi.Select(m => new UpdateMacroFaseDto
            {
                Nome = m.Nome,
                Ordine = m.Ordine
            });

            foreach (var macro in macroFasi)
            {
                Console.WriteLine($"Aggiorno {macro.Nome} → Ordine {macro.Ordine}");

                await Sdk.SendRequestAsync(c => c.UpdateMacroFaseAsync(macro.Id, new UpdateMacroFaseDto
                {
                    Nome = macro.Nome,
                    Ordine = macro.Ordine
                }));
            }

            await AfterNuovaSezione(ProgettoSelezionato);
        }



    }
}
