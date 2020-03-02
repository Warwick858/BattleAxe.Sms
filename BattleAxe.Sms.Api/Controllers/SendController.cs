// ******************************************************************************************************************
//  This file is part of BattleAxe.Sms.
//
//  BattleAxe.Sms - web service that handles all SMS communication.
//  Copyright(C)  2020  James LoForti
//  Contact Info: jamesloforti@gmail.com
//
//  BattleAxe.Sms is free software: you can redistribute it and/or modify
//  it under the terms of the GNU Affero General Public License as published by
//  the Free Software Foundation, either version 3 of the License, or
//  any later version.
//
//  This program is distributed in the hope that it will be useful,
//  but WITHOUT ANY WARRANTY; without even the implied warranty of
//  MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE. See the
//  GNU Affero General Public License for more details.
//
//  You should have received a copy of the GNU Affero General Public License
//  along with this program.If not, see<https://www.gnu.org/licenses/>.
//									     ____.           .____             _____  _______   
//									    |    |           |    |    ____   /  |  | \   _  \  
//									    |    |   ______  |    |   /  _ \ /   |  |_/  /_\  \ 
//									/\__|    |  /_____/  |    |__(  <_> )    ^   /\  \_/   \
//									\________|           |_______ \____/\____   |  \_____  /
//									                             \/          |__|        \/ 
//
// ******************************************************************************************************************
//
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
