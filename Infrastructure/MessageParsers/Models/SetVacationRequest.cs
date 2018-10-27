using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace Messengers.Models
{
    public class SetVacationRequest
    {
        private static readonly Regex RegexQuestion = new Regex(@"ухожу в отпуск с (.+) по (.+)", RegexOptions.Compiled);

        public DateTime From { get; set; }
		public DateTime To { get; set; }

        public static SetVacationRequest TryParse(BotRequest botRequest)
        {
            Match match = RegexQuestion.Match(botRequest.Text);
            if (match.Success)
            {
				DateTime fromDate;
				var fromIsValid = DateTime.TryParseExact(
					match.Groups[1].Value,
					"dd.MM.yyyy",
					CultureInfo.InvariantCulture, DateTimeStyles.None, out fromDate);
				DateTime toDate;
				var toIsValid = DateTime.TryParseExact(
					match.Groups[2].Value,
					"dd.MM.yyyy",
					CultureInfo.InvariantCulture, DateTimeStyles.None, out toDate);
				if (fromIsValid && toIsValid) {
					return new SetVacationRequest() { From = fromDate, To = toDate};
				}
            }

            return null;
        }
    }
}
