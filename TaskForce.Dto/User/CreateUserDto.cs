using System.ComponentModel.DataAnnotations;

namespace TaskForce.Dto.User
{
    public class CreateUserDto
    {
        [Required]
        public string? Nome { get; set; }
    }
}
