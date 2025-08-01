namespace TaskForce.Dto.User
{
    public class UserFasiOverviewDto
    {
        public int UserId { get; set; }
        public string UserNome { get; set; } = null!;
        public UserFaseCorrenteDto? Corrente { get; set; }
        public IEnumerable<UserFaseDto> Completate { get; set; } = Enumerable.Empty<UserFaseDto>();
    }
}
