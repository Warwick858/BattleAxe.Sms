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

namespace BattleAxe.Sms.Common.Model
{
	public class AppSettings : IAppSettings
	{
		public string ApplicationName { get; set; } = string.Empty;
		public string Environment { get; set; } = string.Empty;
		public string SwaggerJsonUrl { get; set; } = string.Empty;
		public string ProviderInUse { get; set; } = string.Empty;
		public string PrimaryProvider { get; set; } = string.Empty;
		public string SecondaryProvider { get; set; } = string.Empty;
		public string TwilioFromPhoneNumber { get; set; } = string.Empty;
		public string TwilioAccountSid { get; set; } = string.Empty;
		public string TwilioAuthToken { get; set; } = string.Empty;
		public bool WhitelistBypass { get; set; }
	} // end class
} // end namespace
