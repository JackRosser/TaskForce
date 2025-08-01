using System.ComponentModel.DataAnnotations;

namespace TaskForce.Dto.Progetto.FasiProgetto.MacroFasi
{
    public class UpdateMacroFaseDto
    {
        public int Id { get; set; }
        public int ProgettoId { get; set; }
        [Required]
        public string? Nome { get; set; }
    }
}
