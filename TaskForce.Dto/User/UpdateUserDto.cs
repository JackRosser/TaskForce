using System.ComponentModel.DataAnnotations;

namespace TaskForce.Dto.User
{
    public class UpdateUserDto
    {
        public int Id { get; set; }
        [Required]
        public string? Nome { get; set; }
    }
}
