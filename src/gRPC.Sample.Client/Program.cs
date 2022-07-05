// See https://aka.ms/new-console-template for more information
using Google.Protobuf.Collections;
using gRPC.Sample.Server;
using Grpc.Net.Client;
using Newtonsoft.Json;

var httpUrl = "http://localhost:5197";
var httpsUrl = "https://localhost:7197";
var channel = GrpcChannel.ForAddress(httpsUrl);

Console.WriteLine("Enter scenario no.\n(1: Greeter, 2: CreateOrder)");
var keyChar = Console.ReadKey().KeyChar;
if (keyChar == '1')
{
    Console.WriteLine("\n--- Greeter selected!");
    var greeterClient = new Greeter.GreeterClient(channel);
    while (true)
    {
        Console.WriteLine("Write a name:");
        var reply = greeterClient.SayHello(new HelloRequest { Name = Console.ReadLine() });
        Console.WriteLine(reply.Message);
        Console.WriteLine("********************************");
    }
}
else if (keyChar == '2')
{
    Console.WriteLine("\n--- Create order selected!");
    var orderClient = new Order.OrderClient(channel);
    while (true)
    {
        var order = new OrderRequest
        {
            CustomerId = new Random().Next(5000, 9000),
        };
        order.OrderItems.AddRange(new OrderItemRequest[]
        {
        new OrderItemRequest
        {
            ProductId = 1,
            Discount = new Random().Next(1, 1000),
            Price = new Random().Next(1000, 9000),
        },
        new OrderItemRequest
        {
            ProductId = 2,
            Discount = new Random().Next(1, 1000),
            Price = new Random().Next(1000, 9000)
        }
        });
        order.OrderTotal = order.OrderItems.Sum(s => s.Price);
        order.OrderDiscount = order.OrderItems.Sum(s => s.Discount);

        var result = await orderClient.CreateOrderAsync(order);
        Console.WriteLine(JsonConvert.SerializeObject(order, Formatting.Indented));
        Console.WriteLine("*** Created an order with id {0} ***", result.OrderId);
        Console.ReadKey();
        Console.Clear();
    }
}
else
{
    Console.WriteLine("Your key is invalid application is closing!");
    Console.ReadKey();
}

