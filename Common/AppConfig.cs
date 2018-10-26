using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Common
{
    public class AppConfig
    {
        public bool RunSlackBot { get; set; }
        public bool RunSkypeBot { get; set; }
        public bool RunTelegramBot { get; set; }
        public string SlackToken { get; set; }
    }
}
