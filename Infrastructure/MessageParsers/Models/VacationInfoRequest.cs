using System.Text.RegularExpressions;

namespace Messengers.Models
{
    public class VacationInfoRequest
    {
        private static readonly Regex RegexQuestion = new Regex(@"(.+)в\sотпуске", RegexOptions.Compiled);

        public string SlackId { get; set; }
        public string FullName { get; set; }

        public static VacationInfoRequest TryParse(BotRequest botRequest)
        {
            Match match = RegexQuestion.Match(botRequest.Text);
            if (match.Success)
            {
				var user = match.Groups[1].Value;
				if (user.StartsWith("<@")) {
					return new VacationInfoRequest() { SlackId = match.Groups[1].Value };
				} else {
					return new VacationInfoRequest() { FullName = match.Groups[1].Value };
				}
            }

            return null;
        }
    }
}
