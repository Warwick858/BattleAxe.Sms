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
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Text.Json;

namespace BattleAxe.Sms.Library.Clients
{
	public class RedisClient : IRedisClient
	{
		private readonly IAppSettings _appSettings;
		private readonly RedisConfig _redisConfig;
		private readonly IConnectionMultiplexer _redis;
		private readonly IDatabase _database;

		public RedisClient(IAppSettings appSettings, RedisConfig redisConfig, IConnectionMultiplexer redis)
		{
			_appSettings = appSettings;
			_redisConfig = redisConfig;
			_redis = redis;
			_database = _redis.GetDatabase();
		}

		public List<string> GetWhitelist(string key)
		{
			Log.Information($"Attempting to get SMS whitelist from redis using key: {key}");

			try
			{
				var numbers = JsonSerializer.Serialize(_database.HashValues(key));
				var whitelist = new List<string>(numbers.Split(';'));

				//Remove brackets from first and last records
				whitelist[0] = whitelist[0].Substring(2);
				whitelist[whitelist.Count - 1] = whitelist[whitelist.Count - 1].Substring(0, whitelist[whitelist.Count - 1].Length - 2);

				return whitelist;
			}
			catch (Exception ex)
			{
				Log.Error($"Failed to get whitelist from redis!");
				throw ex;
			}
		}
	}
}
