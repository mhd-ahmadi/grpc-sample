using Grpc.Core;

namespace gRPC.Sample.Server.Services
{
    public class CustomerService : Customer.CustomerBase
    {
        private readonly ILogger<CustomerService> _logger;
        public CustomerService(ILogger<CustomerService> logger)
        {
            _logger = logger;
        }

        public override async Task GetNewCustomers(GetLookupRequest request, IServerStreamWriter<CustomerModel> responseStream, ServerCallContext context)
        {
            _logger.LogInformation("Calling CustomerService...");
            for (int i = 0; i < 1000; i++)
            {
                var newCustomer = new CustomerModel
                {
                    Id = i + 1,
                    Age = Faker.RandomNumber.Next(18, 100),
                    City = Faker.Address.City(),
                    EmailAddress = Faker.Internet.Email(),
                    FirstName = Faker.Name.First(),
                    LastName = Faker.Name.Last(),
                    Gender = Faker.Enum.Random<Gender>(),
                };
                await responseStream.WriteAsync(newCustomer);
                Thread.Sleep(500);
            }
        }
    }
}