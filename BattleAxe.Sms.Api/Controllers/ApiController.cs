﻿// ******************************************************************************************************************
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
using Microsoft.AspNetCore.Mvc;
using Serilog;
using Swashbuckle.AspNetCore.Annotations;
using System;
using System.Text.Json;

namespace BattleAxe.Sms.Api.Controllers
{
	[Route("[controller]")]
	[ApiController]
	public class ApiController : ControllerBase
	{
		public ApiController()
		{

		}

		[HttpGet]
		[SwaggerOperation(Summary = "Primary endpoint.",
			Description = "This is an example of using swagger annotations!!!!!  See ApiController.cs")]
		public IActionResult Get()
		{
			Log.Information("Attempting to perform GET.");

			try
			{
				//Do something...
				return Ok("GET successful!");
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Failed to perform GET!");
				return BadRequest($"GET failed! Exception: {JsonSerializer.Serialize(ex)}");
			}
		}

		[HttpPost]
		public IActionResult Post([FromBody]GenericRequest request)
		{
			Log.Information("Attempting to perform POST.");

			try
			{
				//Do something...
				return Ok("POST successful!");
			}
			catch (Exception ex)
			{
				Log.Error(ex, "Failed to perform POST!");
				return BadRequest($"POST failed using request: {JsonSerializer.Serialize(request)}! " +
					$"Exception: {JsonSerializer.Serialize(ex)}");
			}
		} // end method
	} // end class
} // end namespace
