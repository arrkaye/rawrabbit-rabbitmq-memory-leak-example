using System;

namespace EasyNetQ.Client
{
    //Client
    public class MyRequest
    {
        public string RequestMessage { get; set; }
    }

    public class MyResponse
    {
        public string ResponseMessage { get; set; }
    }

   
    class Client
    {
        static void Main(string[] args)
        {
            IBus bus = RabbitHutch.CreateBus("host=localhost", x => x.Register<IEasyNetQLogger>(s => new EasyNetQ.Loggers.ConsoleLogger()));

            var response = bus.RequestAsync<MyRequest, MyResponse>(new MyRequest
            {
                RequestMessage = "Hello World!"
            });
            var r = response.Result;
            Console.Out.WriteLine(response.Exception?.Message);
            Console.ReadLine();
        }
    }
}
