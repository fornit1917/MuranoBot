using System;
using Microsoft.AspNetCore.Mvc;

namespace App.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpGet("{token}")]
        public string Register(Guid token)
        {
            return "Hello! " + User.Identity.Name;
        }
    }
}