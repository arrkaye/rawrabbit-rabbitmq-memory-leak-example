using System;
using System.Collections.Generic;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using RawRabbit.Enrichers.MessageContext.Context;
using RawRabbit.Messages.Sample;
using RawRabbit.Operations.MessageSequence;

namespace RawRabbit.AspNet.Sample.Controllers
{
	public class ValuesController : Controller
	{
		private readonly IBusClient _busClient;
		private readonly Random _random;
		private readonly ILogger<ValuesController> _logger;

		public ValuesController(IBusClient legacyBusClient, ILoggerFactory loggerFactory)
		{
			_busClient = legacyBusClient;
			_logger = loggerFactory.CreateLogger<ValuesController>();
			_random = new Random();
		}

		[HttpGet("api/values/{id}")]
		public async Task<string> GetAsync(int id)
		{
			_logger.LogInformation("Requesting Value with id {valueId}", id);
			var response = await _busClient.RequestAsync<ValueRequest, ValueResponse>(new ValueRequest {Value = id});
			return response.Value;
		}
	}
}
