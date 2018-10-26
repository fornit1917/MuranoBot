using System;
using System.Threading.Tasks;
using Quartz;

namespace FoodIntegration
{
	public class OrderIsMadeJob : IJob
	{
		public async Task Execute(IJobExecutionContext context)
		{
			var foodRepo = new FoodRepository();
			DateTime[] actualMenuDates = TempData.ActualMenuDates;
			(string, DateTime[])[] noOrdersForDatesByUser = await foodRepo.NoOrdersForDatesByUser(actualMenuDates);
			if (noOrdersForDatesByUser.Length > 0)
			{
				//notify
			}
		}
	}
}