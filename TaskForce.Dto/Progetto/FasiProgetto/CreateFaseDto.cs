using System.ComponentModel.DataAnnotations;
using TaskForce.Enum;

namespace TaskForce.Dto.Progetto.FasiProgetto
{
    public class CreateFaseDto
    {
        [Required]
        public string? Nome { get; set; }
        [Required]
        public Tipologia? TipoFase { get; set; }
        public int? GiorniPrevisti { get; set; }
    }
}
