using System;
using System.IO;
using System.Linq;

namespace FoodIntegration
{
	public static class TempData
	{
		private const string StoreName = "Store.txt";

		static TempData()
		{
			if (!File.Exists(StoreName))
			{
				var dates = new[] {DateTime.Now.AddDays(-7)};
				ActualMenuDates = dates;
			}
		}

		public static DateTime[] ActualMenuDates
		{
			get { return File.ReadAllLines(StoreName).Select(DateTime.Parse).ToArray(); }
			set { File.WriteAllLines(StoreName, value.Select(d => d.ToShortDateString())); }
		}
	}
}