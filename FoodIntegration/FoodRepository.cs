using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FoodIntegration
{
	public class FoodRepository
	{
		public async Task<DateTime[]> NewMenuDates(DateTime lastMenuDate)
		{
			using (var ctx = new FoodDbContext())
			{
				IQueryable<DateTime> query = ctx.Menus.Where(m => m.Date > lastMenuDate)
					.Select(m => m.Date).Distinct();
				return await query.ToArrayAsync();
			}
		}

		public async Task<Dictionary<string, DateTime[]>> NoOrderDatesByUserEmail(DateTime[] actualMenuDates)
		{
			DateTime menuStartDate = actualMenuDates.Min();
			using (var ctx = new FoodDbContext())
			{
				var userOrdersQuery = from u in ctx.Users
					where u.IsDisabled == false
					select new
					{
						u.Email,
						DatesWithOrder = (from o in ctx.Orders
							join m in ctx.Menus on o.MenuId equals m.Id
							where o.UserId == u.Id && o.Count > 0 && m.Date >= menuStartDate
							select m.Date).Distinct()
					};
				var userOrders = await userOrdersQuery.ToArrayAsync();
				return userOrders.Where(uo => uo.DatesWithOrder.Count() < actualMenuDates.Length)
					.ToDictionary(uo => uo.Email, uo => actualMenuDates.Except(uo.DatesWithOrder).ToArray());
			}
		}
	}
}