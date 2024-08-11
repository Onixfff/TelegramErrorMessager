namespace DataBasePomelo.Models
{
    public class ErrorsEntity
    {
        public Guid Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public EnumsErrorsEntity? EnumsErrors { get; set; }
    }
}
