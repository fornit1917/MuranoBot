﻿using System;
using System.Collections.Generic;
using System.Text;

namespace Messengers.Models
{
    public class BotRequest
    {
        public Messenger Messenger { get; set; }
        public string UserId { get; set; }
        public string ChannelId { get; set; }
        public string Text { get; set; }
    }
}