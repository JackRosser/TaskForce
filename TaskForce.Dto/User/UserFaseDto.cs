namespace TaskForce.Dto.User
{
    public class UserFaseDto
    {
        public int FaseProgettoId { get; set; }
        public string FaseNome { get; set; } = null!;
        public int ProgettoId { get; set; }
        public string? ProgettoNome { get; set; }
        public DateTime DataPresaInCarico { get; set; }
        public DateTime DataFineLavoro { get; set; }
    }
}
