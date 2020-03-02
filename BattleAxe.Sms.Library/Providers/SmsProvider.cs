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
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Text.Json;

namespace BattleAxe.Sms.Library.Providers
{
	public class SmsProvider : ISmsProvider
	{
		private readonly IAppSettings _appSettings;
		private readonly ITwilioClient _twilioClient;
		private readonly Dictionary<string, Func<SmsRequest, string>> _providerRegistration;

		public SmsProvider(IAppSettings appSettings, ITwilioClient twilioClient)
		{
			_appSettings = appSettings;
			_twilioClient = twilioClient;
			_providerRegistration = new Dictionary<string, Func<SmsRequest, string>>
			{
				{ "ALL", Failover },
				{ "TWILIO", _twilioClient.SendSms }
			};
		}

		public string SendSms(SmsRequest smsRequest)
		{
			string response = string.Empty;

			Log.Information($"Received sms message: {JsonSerializer.Serialize(smsRequest)} and are attempting to send using: {_appSettings.ProviderInUse} provider(s).");

			response = _providerRegistration[_appSettings.ProviderInUse].Invoke(smsRequest);

			if (response == "failure")
			{
				Log.Information($"Failed to send sms message!");
			}

			return response;
		}

		[ExcludeFromCodeCoverage]
		private string Failover(SmsRequest smsRequest)
		{
			var response = _providerRegistration[_appSettings.PrimaryProvider].Invoke(smsRequest);

			if (response == "failure")
				response = _providerRegistration[_appSettings.SecondaryProvider].Invoke(smsRequest);

			return response;
		}
	}
}
