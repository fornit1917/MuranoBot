namespace MuranoBot.Common
{
	public static class Utils
	{
		public static string TrimStart(this string trimmedString, string removedString) {
			if (!trimmedString.StartsWith(removedString)) {
				return trimmedString;
			}

			return trimmedString.Substring(removedString.Length);
		}
	}
}