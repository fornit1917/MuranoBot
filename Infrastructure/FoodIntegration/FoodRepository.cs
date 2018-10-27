using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace FoodIntegration
{
	public class FoodRepository
	{
		private readonly FoodDbContext _ctx;

		public FoodRepository(FoodDbContext ctx)
		{
			_ctx = ctx;
		}

		public async Task<DateTime[]> NewMenuDates(DateTime lastMenuDate)
		{
			IQueryable<DateTime> query = _ctx.Menus.Where(m => m.Date > lastMenuDate)
				.Select(m => m.Date).Distinct();
			return await query.ToArrayAsync();
		}

		public async Task<Dictionary<string, DateTime[]>> NoOrderDatesByUserEmail(DateTime[] actualMenuDates)
		{
			DateTime menuStartDate = actualMenuDates.Min();
			var userOrdersQuery = from u in _ctx.Users
				where u.IsDisabled == false
				select new
				{
					u.Email,
					DatesWithOrder = (from o in _ctx.Orders
						join m in _ctx.Menus on o.MenuId equals m.Id
						where o.UserId == u.Id && o.Count > 0 && m.Date >= menuStartDate
						select m.Date).Distinct()
				};
			var userOrders = await userOrdersQuery.ToArrayAsync();
			return userOrders.Where(uo => uo.DatesWithOrder.Count() < actualMenuDates.Length)
				.ToDictionary(uo => uo.Email, uo => actualMenuDates.Except(uo.DatesWithOrder).ToArray());
		}
	}
}