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
using BattleAxe.Sms.Common.Interfaces;
using BattleAxe.Sms.Common.Model;
using BattleAxe.Sms.Library.Interfaces;
using Serilog;
using System;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;
using Twilio.Rest.Api.V2010.Account;
using Twilio.Types;

namespace BattleAxe.Sms.Library.Clients
{
	public class TwilioClient : ITwilioClient
	{
		private readonly IAppSettings _appSettings;
		private readonly Whitelist _whitelist;

		public TwilioClient(IAppSettings appSettings, Whitelist whitelist)
		{
			_appSettings = appSettings;
			_whitelist = whitelist;
		}

		public string SendSms(SmsRequest smsRequest)
		{
			MessageResource? response = null;

			try
			{
				if (IsWhitelisted(smsRequest.To) || _appSettings.WhitelistBypass)
				{
					Twilio.TwilioClient.Init(_appSettings.TwilioAccountSid, _appSettings.TwilioAuthToken);

					response = MessageResource.Create(
						body: smsRequest?.Body,
						from: new PhoneNumber(_appSettings.TwilioFromPhoneNumber),
						to: new PhoneNumber(smsRequest?.To)
					);

					Log.Information($"Sms message processed via Twilio. Request: {JsonSerializer.Serialize(smsRequest)}. " +
						$"Response: {JsonSerializer.Serialize(response)}");
				}
				else
				{
					var msg = $"The given recipient phone number {smsRequest.To} is not whitelisted. SMS message was not sent.";
					Log.Information(msg);

					return msg;
				}

				if (response?.ErrorCode != null)
					Log.Information($"Failed to send sms message via Twilio! Request: {JsonSerializer.Serialize(smsRequest)}. " +
						$"Response: {JsonSerializer.Serialize(response)}");
			}
			catch (Exception ex)
			{
				Log.Information($"Failed to send sms message via Twilio!");
				throw ex;
			}

			return JsonSerializer.Serialize(response);
		}

		private bool IsWhitelisted(string to)
		{
			if (_appSettings?.Environment?.ToUpper() == "PROD")
			{
				return true;
			}
			else if (_whitelist?.List?.Contains(to) ?? false)
			{
				return true;
			}

			return false;
		}
	}
}
