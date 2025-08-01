using System.ComponentModel.DataAnnotations;

namespace TaskForce.Dto.Progetto
{
    public class CreaProgettoDto
    {
        [Required]
        public string? Nome { get; set; }
        [Required]
        public DateTime Consegna { get; set; }
    }
}
