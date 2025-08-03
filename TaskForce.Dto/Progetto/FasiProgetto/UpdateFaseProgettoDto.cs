using System.ComponentModel.DataAnnotations;
using TaskForce.Enum;

namespace TaskForce.Dto.Progetto.FasiProgetto
{
    public class UpdateFaseProgettoDto
    {
        [Required]
        public string Nome { get; set; } = null!;
        public int? GiorniPrevisti { get; set; }
        [Required]
        public Tipologia TipoFase { get; set; }
        [Required]
        public StatoFase Stato { get; set; }
    }
}
