namespace WebApplication1.Model
{
    sealed class DeliveryAttempt
    {
        public int Id { get; set; }

        public int AlertId { get; set; }

        public required string Channel { get; set; }

        public required string EventTitle { get; set; }

        public required string Status { get; set; }

        public string? Error { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}
