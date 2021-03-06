﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using EasyNetQ;
using RawRabbit.Common;
using RawRabbit.Instantiation;
using RawRabbit.Messages.Sample;

namespace RawRabbit.ConsoleApp.Sample
{
	public class Program
	{
		private IBusClient _client;
	    static readonly ManualResetEvent QuitEvent = new ManualResetEvent(false);

        public static void Main(string[] args)
        {
//            var client = RabbitHutch.CreateBus("host=localhost;username=guest;password=guest");
            IBusClient client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions() { ClientConfiguration = ConnectionStringParser.Parse("guest:guest@localhost:5672/") });

            client.RespondAsync<ValueRequest, ValueResponse>(r => SendValuesThoughRpcAsync(r));

            QuitEvent.WaitOne();
        }

	    private static Task<ValueResponse> SendValuesThoughRpcAsync(ValueRequest request)
		{
            Console.Out.WriteLine($"Responding to request: {request.Value}");
		    var randomString = RandomString(100000);
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
