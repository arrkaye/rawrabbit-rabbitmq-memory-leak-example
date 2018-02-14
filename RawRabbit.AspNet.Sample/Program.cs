using System;
using System.Collections.Generic;
using System.IO;
using Microsoft.AspNetCore.Hosting;
using RawRabbit.Common;
using RawRabbit.Instantiation;
using RawRabbit.Messages.Sample;

namespace RawRabbit.AspNet.Sample
{
	public class Program
	{
		public static void Main(string[] args)
		{
		    IBusClient client = RawRabbitFactory.CreateSingleton(new RawRabbitOptions(){ClientConfiguration = ConnectionStringParser.Parse("guest:guest@localhost:5672/")});

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
