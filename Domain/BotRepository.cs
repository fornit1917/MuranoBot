using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace Domain
{
	public class BotRepository
	{
		public async Task<ILookup<string, (Messenger Messenger, string ExternalId)>> GetExternalIdByUserEmail(ICollection<string> emails)
		{
			using (var ctx = new DomainDbContext())
			{
				var query = from u in ctx.Users
					join m in ctx.MessengerLinks on u.Id equals m.UserId
					where emails.Contains(u.Email)
					select new {u.Email, m.ExternalUserId, m.Messenger};
				var result = await query.ToArrayAsync();
				return result.ToLookup(r => r.Email, r => (r.Messenger, r.ExternalUserId));
			}
		}

		public async Task<bool> IsLinkRegistered(Messenger messenger, string externalId)
		{
			using (var ctx = new DomainDbContext())
			{
				return await ctx.MessengerLinks.AnyAsync(l =>l.Messenger == messenger && l.ExternalUserId == externalId && l.UserId.HasValue);
			}
		}

		public async Task<MessengerLink> GetLinkByAuthToken(Guid authToken) {
			using (var ctx = new DomainDbContext()) {
				return await GetLinkByAuthToken(ctx, authToken);
			}
		}

		public async Task<Guid> RegisterLink(Messenger messenger, string externalId)
		{
			using (var ctx = new DomainDbContext())
			{
				MessengerLink existingLink = await ctx.MessengerLinks.SingleOrDefaultAsync(l => l.Messenger == messenger && l.ExternalUserId == externalId);
				if (existingLink != null)
				{
					return existingLink.AuthToken;
				}
				Guid authToken = Guid.NewGuid();
				ctx.MessengerLinks.Add(new MessengerLink{AuthToken = authToken,ExternalUserId = externalId,Messenger = messenger});
				await ctx.SaveChangesAsync();
				return authToken;
			}
		}

		public async Task RegisterUser(int id, string email, Guid authToken)
		{
			using (var ctx = new DomainDbContext())
			{
				ctx.Users.Add(new User {Email = email, Id = id});
				await ctx.SaveChangesAsync();
				MessengerLink link = await GetLinkByAuthToken(ctx, authToken);
				link.UserId = id;
				await ctx.SaveChangesAsync();
			}
		}

		private Task<MessengerLink> GetLinkByAuthToken(DomainDbContext ctx, Guid authToken)
		{
			return ctx.MessengerLinks.SingleOrDefaultAsync(l => l.AuthToken == authToken);
		}
	}
}