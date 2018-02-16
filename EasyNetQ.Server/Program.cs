using System;
using System.Threading.Tasks;
using EasyNetQ.Client;

namespace EasyNetQ.Server
{
    class Server
    {
        static void Main(string[] args)
        {
            IBus bus = RabbitHutch.CreateBus("host=localhost", x => x.Register<IEasyNetQLogger>(s => new EasyNetQ.Loggers.ConsoleLogger()));
            var i = 0;
            var response = bus.RespondAsync<MyRequest, MyResponse>(async request =>
            {
                Console.Out.WriteLine($"Throwing {++i} about {request.RequestMessage}");
                await ThrowException(i);
                return new MyResponse()
                {
                    ResponseMessage = i.ToString()
                };
            });
        }

        private async static Task ThrowException(int i)
        {
            throw new Exception("INNER: " + i.ToString());
        }
    }

}
