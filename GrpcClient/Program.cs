using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Grpc.Core;
using Helloworld;

namespace GrpcClient
{
    class Program
    {
        public static void Main(string[] args)
        {
            Channel channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);

            var client = new Greeter.GreeterClient(channel);

            var reply = client.SayHello(new HelloRequest { Name = "Roger" });
            Console.WriteLine("Greeting: " + reply.Message);

            channel.ShutdownAsync().Wait();
            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
