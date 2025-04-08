namespace HairSalonApi.Models
{
    public class Service
    {
        public Guid Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public string Category { get; set; }
        public TimeSpan Duration { get; set; }
    }
}
