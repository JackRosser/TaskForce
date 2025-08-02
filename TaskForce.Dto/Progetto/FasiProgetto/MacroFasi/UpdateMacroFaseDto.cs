using System.ComponentModel.DataAnnotations;

namespace TaskForce.Dto.Progetto.FasiProgetto.MacroFasi
{
    public class UpdateMacroFaseDto
    {
        [Required]
        public string? Nome { get; set; }
    }
}
