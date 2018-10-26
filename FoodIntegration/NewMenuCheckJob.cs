using System;
using System.Linq;
using System.Threading.Tasks;
using Quartz;

namespace FoodIntegration
{
	public class NewMenuCheckJob : IJob
	{
		public async Task Execute(IJobExecutionContext context)
		{
			var foodRepo = new FoodRepository();
			DateTime[] actualMenuDates = TempData.ActualMenuDates;
			DateTime[] newMenuDates = await foodRepo.NewMenuDates(actualMenuDates.Max());
			if (newMenuDates.Length > 0)
			{
				TempData.ActualMenuDates = newMenuDates;
				//notify
			}
		}
	}
}