using TaskForce.Enum;

namespace TaskForce.Dto.Progetto.FasiProgetto
{
    public class GetFaseDto
    {
        public int Id { get; set; }
        public int MacroFaseId { get; set; }
        public string Nome { get; set; } = null!;
        public int GiorniPrevisti { get; set; }
        public Tipologia TipoFase { get; set; }
        public StatoFase Stato { get; set; }
    }
}
