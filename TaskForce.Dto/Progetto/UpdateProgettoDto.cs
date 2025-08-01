using System.ComponentModel.DataAnnotations;

namespace TaskForce.Dto.Progetto
{
    public class UpdateProgettoDto
    {
        public int Id { get; set; }
        [Required]
        public string? Nome { get; set; }
        [Required]
        public DateTime Consegna { get; set; }
    }
}
