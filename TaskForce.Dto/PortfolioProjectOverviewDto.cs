using TaskForce.Enum;

namespace TaskForce.Dto
{
    public class PortfolioProjectOverviewDto
    {
        public int ProgettoId { get; set; }
        public string Nome { get; set; } = null!;
        public DateTime Consegna { get; set; }

        public int GiorniInseriti { get; set; }
        public int GiorniLavorati { get; set; }
        public int GiorniRimanenti { get; set; }

        public int GiorniLavorativiDisponibiliTot { get; set; }
        public int GiorniLavorativiDisponibiliNetti { get; set; }

        public int SlackGiorni { get; set; }
        public TipoEsito Esito { get; set; }
    }
}
