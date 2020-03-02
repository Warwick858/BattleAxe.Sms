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
using BattleAxe.Sms.Common.Interfaces;
using BattleAxe.Sms.Common.Model;
using BattleAxe.Sms.Library.Clients;
using BattleAxe.Sms.Library.Interfaces;
using BattleAxe.Sms.Library.Providers;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using Serilog;
using StackExchange.Redis;
using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Headers;

namespace BattleAxe.Sms.Api
{
	public class Startup
	{
		private IAppSettings? _appSettings;

		private IConfiguration Configuration { get; }

		public Startup(IConfiguration configuration)
		{
			Configuration = configuration;
		}

		public void ConfigureServices(IServiceCollection services)
		{
			_appSettings = Configuration.GetSection("AppSettings").Get<AppSettings>();
			services.AddSingleton(_appSettings);
			services.AddControllers();
			services.AddHttpContextAccessor();
			services.AddSwaggerGen(c =>
			{
				c.EnableAnnotations();
				c.SwaggerDoc("v1", new OpenApiInfo
				{
					Version = "v1",
					Title = _appSettings.ApplicationName,
					Description = "Web service that handles all SMS communication.",
					Contact = new OpenApiContact()
					{
						Name = "James LoForti",
						Email = "jamesloforti@gmail.com",
						Url = new Uri("http://www.jimmyloforti.com")
					},
					License = new OpenApiLicense
					{
						Name = "GNU Affero General Public License",
						Url = new Uri("https://www.gnu.org/licenses/"),
					}
				});
			});

			services.AddSingleton<ITwilioClient, TwilioClient>();
			services.AddSingleton<ISmsProvider, SmsProvider>();

			ConfigureRedis(services);

			var httpClient = new HttpClient();
			httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
			services.AddSingleton(h => httpClient);

			Log.Information($"{_appSettings.ApplicationName} has started successfully.");
		} // end method

		public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
		{
			if (env.IsDevelopment())
				app.UseDeveloperExceptionPage();
			else
				app.UseHsts();

			app.UseStaticFiles();
			app.UseHttpsRedirection();
			app.UseRouting();
			app.UseEndpoints(endpoints =>
			{
				endpoints.MapControllers();
			});
			app.UseSwagger();
			app.UseSwaggerUI(c =>
			{
				c.SwaggerEndpoint(_appSettings!.SwaggerJsonUrl, _appSettings.ApplicationName);
				c.DocumentTitle = _appSettings.ApplicationName; // swagger ui web page title
				c.RoutePrefix = "swagger"; // swagger ui url path (also see launchSettings under Properties)
			});
		} // end method

		private void ConfigureRedis(IServiceCollection services)
		{
			Whitelist whitelist = new Whitelist
			{
				List = new List<string>()
			};

			if (_appSettings!.Environment.ToLower() == "prod")
			{
				var redisConfig = new RedisConfig();
				Configuration.GetSection("RedisConfig").Bind(redisConfig);
				using var redis = ConnectionMultiplexer.Connect(redisConfig.ConnectionString);
				var redisClient = new RedisClient(_appSettings!, redisConfig, redis);
				whitelist = new Whitelist { List = redisClient.GetWhitelist(redisConfig.WhitelistLocation) };
			}

			services.AddSingleton(whitelist);

		} // end method
	} // end class
} // end namespace
