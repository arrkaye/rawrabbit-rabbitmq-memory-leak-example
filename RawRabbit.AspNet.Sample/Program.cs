using System;
using System.Collections.Generic;
using EasyNetQ;
using RawRabbit.Messages.Sample;

namespace RawRabbit.AspNet.Sample
{
	public class Program
	{
		public static void Main(string[] args)
		{
            //		    IBusClient client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions(){ClientConfiguration = ConnectionStringParser.Parse("guest:guest@localhost:5672/")});

		    var client = RabbitHutch.CreateBus("host=localhost;username=guest;password=guest");

            Console.Out.WriteLine($"Created client. Press enter to start.");
		    Console.ReadLine();
		    var memory = new List<long>();
            for (var i = 0; i < 10000; i++)
		    {
                if ((i % 1000) == 0)
		        {
                    Console.Out.WriteLine($"Paused at iteration: {i}. Press enter to GC.Collect() and continue.");
		            Console.ReadLine();
                    memory.Add(GC.GetTotalMemory(false));
                    GC.Collect();
		        }

		        var result = client.RequestAsync<ValueRequest, ValueResponse>(new ValueRequest {Value = i}).Result;
		    }

            Console.Out.WriteLine(string.Join(",", memory));
            Console.Out.WriteLine("Finished.");
		    Console.ReadLine();
		}
	}
}
