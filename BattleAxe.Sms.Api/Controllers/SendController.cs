using BattleAxe.Sms.Common.Model;
using BattleAxe.Sms.Library.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Serilog;
using System;
using System.Text.Json;

namespace BattleAxe.Sms.Api.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class SendController : ControllerBase
	{
		private readonly ISmsProvider _smsProvider;

		public SendController(ISmsProvider smsProvider)
		{
			_smsProvider = smsProvider;
		}

		[HttpPost]
		public IActionResult Post([FromBody]SmsRequest smsRequest)
		{
			Log.Information($"Received sms message: {JsonSerializer.Serialize(smsRequest)} and are attempting send.");

			try
			{
				return Ok(_smsProvider.SendSms(smsRequest));
			}
			catch (Exception ex)
			{
				Log.Information($"Failure! Sms message: {JsonSerializer.Serialize(smsRequest)} did not send.");
				return Ok(ex);
			}
		}
	}
}
