namespace DataBasePomelo.Models
{
    public class ErrorsEntity
    {
        public int Id { get; set; }
        public string Message { get; set; } = string.Empty;
        public string Level { get; set; }
        public DateTime Date { get; set; }
        public string Exception { get; set; }
    }
}
