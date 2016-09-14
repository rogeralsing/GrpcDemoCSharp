using System;
using System.Threading.Tasks;
using Grpc.Core;
using Grpc.Core.Utils;
using Helloworld;

namespace GrpcServer
{
    class GreeterImpl : Greeter.GreeterBase
    {
        // Server side handler of the SayHello RPC
        public override async Task<HelloReply> SayHello(HelloRequest request, ServerCallContext context)
        {
            return new HelloReply { Message = "Hello " + request.Name };
        }

        public override async Task<HelloReply> SayHelloStream(IAsyncStreamReader<HelloRequest> requestStream, ServerCallContext context)
        {
            await requestStream.ForEachAsync(i => Task.FromResult(0));
           
            return new HelloReply { Message = "Hello"};
        }
    }

    class Program
    {
        const int Port = 50051;

        public static void Main(string[] args)
        {
            Server server = new Server
            {                
                Services = { Greeter.BindService(new GreeterImpl()) },
                Ports = { new ServerPort("0.0.0.0", Port, ServerCredentials.Insecure) }
            };
            server.Start();			

            Console.WriteLine("Greeter server listening on port " + Port);
			//Console.WriteLine("Press any key to stop the server...");
			//Console.ReadKey();

			System.Threading.Thread.Sleep(30000);

			Console.WriteLine("Server is shutting down..");
            server.ShutdownAsync().Wait();
        }
    }
}
