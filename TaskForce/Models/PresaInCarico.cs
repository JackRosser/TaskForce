namespace TaskForce.Models
{
    public class PresaInCarico
    {
        public int Id { get; set; }
        public int FaseProgettoId { get; set; }
        public int UserId { get; set; }
        public DateTime DataPresaInCarico { get; set; }
        public DateTime? DataFineLavoro { get; set; }
    }
}
