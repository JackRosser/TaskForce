using System.ComponentModel.DataAnnotations;

namespace TaskForce.Dto.Progetto.FasiProgetto.MacroFasi
{
    public class CreateMacroFaseDto
    {
        [Required]
        public string? Nome { get; set; }
    }
}
