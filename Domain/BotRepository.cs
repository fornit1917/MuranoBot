using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
	public class BotRepository
	{
		public async Task<ILookup<string, (Messenger Messenger, string ExternalId)>> GetExternalIdByEmail(ICollection<string> emails)
		{
			using (var ctx = new DomainDbContext())
			{
				var query = from u in ctx.Users
					join m in ctx.MessengerLinks on u.Id equals m.UserId
					where emails.Contains(u.Email)
					select new {u.Email, m.ExternalUserId, m.MessengerId};
				var result = await query.ToArrayAsync();
				return result.ToLookup(r => r.Email, r => (r.MessengerId, r.ExternalUserId));
			}
		}
	}
}