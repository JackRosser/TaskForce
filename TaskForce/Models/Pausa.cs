namespace TaskForce.Models
{
    public class Pausa
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PresaInCaricoId { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime? DataFine { get; set; }

    }
}
