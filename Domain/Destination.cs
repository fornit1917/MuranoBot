using Domain;

namespace Domain 
{
    public class Destination
    {
        public Messenger Messenger { get; set; }
        public string ChannelId { get; set; }
        public string UserId { get; set; }
    }
}
