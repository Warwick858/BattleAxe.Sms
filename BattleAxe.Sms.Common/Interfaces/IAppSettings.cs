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
namespace BattleAxe.Sms.Common.Interfaces
{
	public interface IAppSettings
	{
		string ApplicationName { get; set; }
		string Environment { get; set; }
		string SwaggerJsonUrl { get; set; }
		string ProviderInUse { get; set; }
		string PrimaryProvider { get; set; }
		string SecondaryProvider { get; set; }
		string TwilioFromPhoneNumber { get; set; }
		string TwilioAccountSid { get; set; }
		string TwilioAuthToken { get; set; }
		bool WhitelistBypass { get; set; }
	} // end interface
} // end namespace
