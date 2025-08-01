using System.ComponentModel.DataAnnotations;

namespace TaskForce.Dto.Progetto.FasiProgetto
{
    public class CreateFaseDto
    {
        [Required]
        public string? Nome { get; set; }
        public int? GiorniPrevistiBe { get; set; }
        public int? GiorniPrevistiUi { get; set; }
    }
}
