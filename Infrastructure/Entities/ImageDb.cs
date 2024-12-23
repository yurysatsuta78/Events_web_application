namespace Infrastructure.Entities
{
    public class ImageDb
    {
        public int Id { get; set; }
        public string ImagePath { get; set; } = string.Empty;
        public EventDb? Event {  get; set; }
        public Guid EventId { get; set; }
    }
}
