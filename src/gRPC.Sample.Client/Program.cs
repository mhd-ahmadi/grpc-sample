// See https://aka.ms/new-console-template for more information
using Google.Protobuf.Collections;
using gRPC.Sample.Server;
using Grpc.Core;
using Grpc.Net.Client;
using Newtonsoft.Json;

//var httpUrl = "http://localhost:5197";
var httpsUrl = "https://localhost:7197";
var channel = GrpcChannel.ForAddress(httpsUrl);

Console.WriteLine("Enter scenario no.\n(1: Greeter, 2: CreateOrder, 3: GetNewCustomers)");
var keyChar = Console.ReadKey().KeyChar;
if (keyChar == '1')
{
    Console.WriteLine("\n--- Greeter selected!");
    Greeter();
}
else if (keyChar == '2')
{
    Console.WriteLine("\n--- Create order selected!");
    await CreateNewOrder();
}
else if (keyChar == '3')
{
    Console.WriteLine("\n--- Get new customers selected!");
    await GetNewCustomers();
}
else
{
    Console.WriteLine("Your key is invalid application is closing!");
    Console.ReadKey();
}

void Greeter()
{   
    var client = new Greeter.GreeterClient(channel);
    while (true)
    {
        Console.WriteLine("Write a name:");
        var reply = client.SayHello(new HelloRequest { Name = Console.ReadLine() });
        Console.WriteLine(reply.Message);
        Console.WriteLine("********************************");
    }
}

async Task CreateNewOrder()
{   
    var client = new Order.OrderClient(channel);
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

        var result = await client.CreateOrderAsync(order);
        Console.WriteLine(JsonConvert.SerializeObject(order, Formatting.Indented));
        Console.WriteLine("*** Created an order with id {0} ***", result.OrderId);
        Console.ReadKey();
        Console.Clear();
    }
}

async Task GetNewCustomers()
{
    var client = new Customer.CustomerClient(channel);
    using var call = client.GetNewCustomers(new GetLookupRequest());
    while (await call.ResponseStream.MoveNext())
    {
        var customer = call.ResponseStream.Current;
        Console.WriteLine($"*** New Customer created with id = {customer.Id} ***");
        Console.WriteLine($"First Name = {customer.FirstName}");
        Console.WriteLine($"LastName Name = {customer.LastName}");
        Console.WriteLine($"Age = {customer.Age}");
        Console.WriteLine($"Gender = {customer.Gender}");
        Console.WriteLine($"City = {customer.City}");
        Console.WriteLine($"Email Address = {customer.EmailAddress}");
        Console.WriteLine($"--------------------------------------------------------");
    }
    Console.WriteLine("Reach to end of the list...");
    Console.ReadKey();
}