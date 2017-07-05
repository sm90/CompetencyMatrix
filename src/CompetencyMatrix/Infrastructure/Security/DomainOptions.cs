namespace CompetencyMatrix.Infrastructure.Security
{
    public class DomainOptions
    {
        public DomainOptions()
        {
            DomainControllers = new string[0];
        }

        public string[] DomainControllers { get; set; }
    }
}