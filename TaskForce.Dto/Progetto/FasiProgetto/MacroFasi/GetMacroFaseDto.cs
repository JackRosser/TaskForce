namespace TaskForce.Dto.Progetto.FasiProgetto.MacroFasi
{
    public class GetMacroFaseDto
    {
        public int Id { get; set; }
        public int ProgettoId { get; set; }
        public string Nome { get; set; } = null!;
        public int Ordine { get; set; }
    }

}
