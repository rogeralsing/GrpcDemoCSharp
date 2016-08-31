using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Utils;
using Helloworld;

namespace GrpcClient
{
    class Program
    {
        public static void Main(string[] args)
        {
            Thread.Sleep(2000); //just wait for server to start

            Channel channel = new Channel("127.0.0.1:50051", ChannelCredentials.Insecure);
            
            var client = new Greeter.GreeterClient(channel);

            Console.WriteLine("Executing synchronously, 10 000 calls");
            var sw = Stopwatch.StartNew();
            for (int i = 0; i < 10000; i++)
            {
                client.SayHello(new HelloRequest { Name = "Roger" });
            }
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            Console.WriteLine("Executing asynchronously, 50 000 calls");
            var l = new List<Task>();
            sw = Stopwatch.StartNew();
            for (int i = 0; i < 50000; i++)
            {
                var t = Task.Run(async () => await client.SayHelloAsync(new HelloRequest { Name = "Roger" }));
                l.Add(t);
            }

            Task.WaitAll(l.ToArray());
            sw.Stop();
            Console.WriteLine(sw.Elapsed);


            Console.WriteLine("Executing stream, 50 000 calls");
            sw = Stopwatch.StartNew();
            var x = client.SayHelloStream();
            var batch = Enumerable.Repeat(new HelloRequest {Name = "Roger"}, 50000);
            x.RequestStream.WriteAllAsync(batch).Wait();
            x.ResponseAsync.Wait();
            sw.Stop();
            Console.WriteLine(sw.Elapsed);

            channel.ShutdownAsync().Wait();
            Console.ReadKey();
        }
    }
}
