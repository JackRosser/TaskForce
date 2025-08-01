namespace TaskForce.Models
{
    public class Progetto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public DateTime Consegna { get; set; }
    }
}
