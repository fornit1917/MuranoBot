using Messengers.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Common;
using SlackAPI;

namespace Messengers.Services
{
    public class MessageHandler
    {
        private SlackTaskClient _slackClient;

        public MessageHandler(AppConfig appConfig)
        {
            _slackClient = new SlackTaskClient(appConfig.SlackToken);
        }

        public Task HandleRequestAsync(BotRequest botRequest)
        {
            var vacationInfoRequest = VacationInfoRequest.TryParse(botRequest);
            if (vacationInfoRequest != null)
            {
                return _slackClient.PostMessageAsync(botRequest.ChannelId, "Запрос информации об отпуске для сотрудника " + vacationInfoRequest.Name);
            }

            return _slackClient.PostMessageAsync(botRequest.ChannelId, "Неизвестный запрос: " + botRequest.Text);
        }
    }
}
