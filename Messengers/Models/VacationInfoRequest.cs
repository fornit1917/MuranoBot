using System.Text.RegularExpressions;

namespace Messengers.Models
{
    public class VacationInfoRequest
    {
        private static readonly Regex RegexQuestion = new Regex(@"(.+)в\sотпуске", RegexOptions.Compiled);

        public string Name { get; set; }

        public static VacationInfoRequest TryParse(BotRequest botRequest)
        {
            Match match = RegexQuestion.Match(botRequest.Text);
            if (match.Success)
            {
                return new VacationInfoRequest() { Name = match.Groups[1].Value };
            }

            return null;
        }
    }
}
