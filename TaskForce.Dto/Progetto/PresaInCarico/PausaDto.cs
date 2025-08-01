namespace TaskForce.Dto.Progetto.PresaInCarico
{
    public class PausaDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public int PresaInCaricoId { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime? DataFine { get; set; }
    }
}
