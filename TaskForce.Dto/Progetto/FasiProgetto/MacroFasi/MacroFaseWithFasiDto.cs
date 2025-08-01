namespace TaskForce.Dto.Progetto.FasiProgetto.MacroFasi;

public class MacroFaseWithFasiDto
{
    public int Id { get; set; }
    public int ProgettoId { get; set; }
    public string? Nome { get; set; }
    public IEnumerable<GetFaseDto> Fasi { get; set; } = Enumerable.Empty<GetFaseDto>();
}
