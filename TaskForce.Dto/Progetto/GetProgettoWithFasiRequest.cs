using TaskForce.Enum;

namespace TaskForce.Dto.Progetto
{
    public class GetProgettoWithFasiRequest
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public DateTime Consegna { get; set; }

        public IEnumerable<GetMacroFaseDettaglioDto>? MacroFasi { get; set; }
    }

    public class GetMacroFaseDettaglioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;

        public IEnumerable<GetFaseDettaglioDto>? Fasi { get; set; }
    }

    public class GetFaseDettaglioDto
    {
        public int Id { get; set; }
        public string Nome { get; set; } = null!;
        public int GiorniPrevisti { get; set; }
        public StatoFase Stato { get; set; }
        public Tipologia TipoFase { get; set; }

        public IEnumerable<GetPresaInCaricoDettaglioDto>? PreseInCarico { get; set; }
    }

    public class GetPresaInCaricoDettaglioDto
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string UserName { get; set; } = null!;
        public StatoUtente Stato { get; set; }
        public DateTime DataInizio { get; set; }
        public DateTime? DataFine { get; set; }

        public int GiorniEffettivi { get; set; }
        public int GiorniPrevisti { get; set; }

        public int DeltaGiorni => GiorniEffettivi - GiorniPrevisti;
        public string? DeltaTempi => Stato == StatoUtente.Concluso ? DeltaGiorni switch
        {
            > 0 => $"+{DeltaGiorni}g",
            < 0 => $"-{Math.Abs(DeltaGiorni)}g",
            _ => "Allineamento"
        }
    : null;


    }


}
