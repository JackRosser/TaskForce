using System.ComponentModel.DataAnnotations;
using TaskForce.Enum;

namespace TaskForce.Dto.Progetto.FasiProgetto
{
    public class UpdateFaseProgettoDto
    {
        public int Id { get; set; }
        public int ProgettoId { get; set; }
        public int MacroFaseId { get; set; }
        [Required]
        public string Nome { get; set; } = null!;
        public int? GiorniPrevisti { get; set; }
        [Required]
        public Tipologia TipoFase { get; set; }
        public StatoFase Stato { get; set; }
    }
}
