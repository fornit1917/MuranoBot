using System;
using System.Threading.Tasks;
using MuranoBot.Domain;
using Microsoft.AspNetCore.Mvc;
using MuranoBot.Infrastructure.TimeTracking.App.Application;
using MuranoBot.Infrastructure.TimeTracking.App.Application.Models;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
	    private readonly UsersApp _usersApp;
	    private readonly BotRepository _botRepository;

	    public AuthController(UsersApp usersApp, BotRepository botRepository)
	    {
		    _usersApp = usersApp;
		    _botRepository = botRepository;
	    }

	    [HttpGet("{token}")]
        public async Task<string> Register(Guid token)
        {
            MessengerLink link = await _botRepository.GetLinkByAuthToken(token);
	        if (link.UserId.HasValue)
	        {
		        return "You are already registered!";
	        }
	        string domainName = User.Identity.Name;
			UserInfo ttUser = _usersApp.GetUserInfo(domainName);
	        if (ttUser != null)
	        {
		        await _botRepository.RegisterUser(ttUser.Id, ttUser.Email, token);
		        return "Successfully registered!";
	        }
	        return $"No user with domain name '{domainName}' in TimeTracker";
        }
    }
}