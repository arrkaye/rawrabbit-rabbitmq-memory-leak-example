using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using RawRabbit.Configuration;
using RawRabbit.Enrichers.GlobalExecutionId;
using RawRabbit.Enrichers.MessageContext;
using RawRabbit.Enrichers.MessageContext.Context;
using RawRabbit.Instantiation;
using RawRabbit.Messages.Sample;
using Serilog;

namespace RawRabbit.ConsoleApp.Sample
{
	public class Program
	{
		private IBusClient _client;
	    static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);

        public static void Main(string[] args)
        {
            new Program().Start();
            QuitEvent.WaitOne();
        }

		public async Task Start()
		{
			Log.Logger = new LoggerConfiguration()
				.WriteTo.LiterateConsole()
				.CreateLogger();

			_client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions
			{
				ClientConfiguration = new ConfigurationBuilder()
					.SetBasePath(Directory.GetCurrentDirectory())
					.AddJsonFile("rawrabbit.json")
					.Build()
					.Get<RawRabbitConfiguration>(),
				Plugins = p => p
					.UseGlobalExecutionId()
					.UseMessageContext<MessageContext>()
			});

			await _client.RespondAsync<ValueRequest, ValueResponse>(request => SendValuesThoughRpcAsync(request));
		}

		private Task<ValueResponse> SendValuesThoughRpcAsync(ValueRequest request)
		{
		    var randomString = RandomString(1000000);
            Console.Out.WriteLine(randomString);
		    return Task.FromResult(new ValueResponse
			{
				Value = randomString
			});
		}

	    private static Random random = new Random();
	    public static string RandomString(int length)
	    {
	        const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
	        return new string(Enumerable.Repeat(chars, length)
	            .Select(s => s[random.Next(s.Length)]).ToArray());
	    }
    }
}
