using MuranoBot.Domain;

namespace MuranoBot.Infrastructure.MessageSenders.Models
{
    public class Destination
    {
        public Messenger Messenger { get; set; }
        public string ChannelId { get; set; }
        public string UserId { get; set; }
    }
}
