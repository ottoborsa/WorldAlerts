namespace WebApplication1.Model
{
    sealed class AlertChannel
    {
        public int Id { get; set; }

        public int AlertId { get; set; }

        public required string Type { get; set; }
    }
}
