namespace WebApplication1.Model
{
    sealed class Alert
    {
        public int Id { get; set; }

        public required string UserId { get; set; }

        public required string Name { get; set; }

        public List<string> Keywords { get; set; } = [];

        public List<string> Categories { get; set; } = [];

        public bool Enabled { get; set; } = true;

        public List<AlertChannel> Channels { get; set; } = [];
    }
}
