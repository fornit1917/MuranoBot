using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;

namespace Messengers.Models
{
    class VacationInfoRequest
    {
        private static Regex regexQuestion = new Regex(@"(.+)в\sотпуске");

        public string Name { get; set; }

        public static VacationInfoRequest TryParse(BotRequest botRequest)
        {
            Match match = regexQuestion.Match(botRequest.Text);
            if (match.Success)
            {
                return new VacationInfoRequest() { Name = match.Groups[1].Value };
            }

            return null;
        }


    }
}
