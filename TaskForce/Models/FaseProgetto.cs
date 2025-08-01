using TaskForce.Enum;

namespace TaskForce.Models
{
    public class FaseProgetto
    {
        public int Id { get; set; }
        public int MacroFaseId { get; set; }
        public string Nome { get; set; } = null!;
        public int? GiorniPrevistiBe { get; set; }
        public int? GiorniPrevistiUi { get; set; }
        public StatoFase Stato { get; set; }
    }
}
