using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace BattleAxe.Sms.Api.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ServiceStatusController : ControllerBase
	{
		public ServiceStatusController()
		{
		}

		[HttpGet]
		public IActionResult Get()
		{
			Log.Information("BattleAxe.Sms status check.");
			return Ok($"The BattleAxe.Sms service is up and running.");
		}
	}
}
