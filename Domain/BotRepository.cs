using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace MuranoBot.Domain
{
	public class BotRepository
	{
		private readonly DomainDbContext _ctx;

		public BotRepository(DomainDbContext ctx)
		{
			_ctx = ctx;
		}

		public async Task<ILookup<string, (Messenger Messenger, string ExternalId)>> GetExternalIdByUserEmail(ICollection<string> emails) {
			var query = from u in _ctx.Users
				join m in _ctx.MessengerLinks on u.Id equals m.UserId
				where emails.Contains(u.Email)
				select new {u.Email, m.ExternalUserId, m.Messenger};
			var result = await query.ToArrayAsync();
			return result.ToLookup(r => r.Email, r => (r.Messenger, r.ExternalUserId));
		}

		public async Task<bool> IsLinkRegistered(Messenger messenger, string externalId)
		{
			return await _ctx.MessengerLinks.AnyAsync(l => l.Messenger == messenger && l.ExternalUserId == externalId && l.UserId.HasValue);
		}

		public async Task<MessengerLink> GetLinkByAuthToken(Guid authToken)
		{
			return await _ctx.MessengerLinks.SingleOrDefaultAsync(l => l.AuthToken == authToken);
		}

		public async Task<Guid> RegisterLink(Messenger messenger, string externalId)
		{
			MessengerLink existingLink = await _ctx.MessengerLinks.SingleOrDefaultAsync(l => l.Messenger == messenger && l.ExternalUserId == externalId);
			if (existingLink != null)
			{
				return existingLink.AuthToken;
			}

			Guid authToken = Guid.NewGuid();
			_ctx.MessengerLinks.Add(new MessengerLink{AuthToken = authToken, ExternalUserId = externalId, Messenger = messenger});
			await _ctx.SaveChangesAsync();
			return authToken;
		}

		public async Task RegisterUser(int id, string email, Guid authToken)
		{
			_ctx.Users.Add(new User {Email = email, Id = id});
			await _ctx.SaveChangesAsync();
			MessengerLink link = await GetLinkByAuthToken(authToken);
			link.UserId = id;
			await _ctx.SaveChangesAsync();
		}
	}
}