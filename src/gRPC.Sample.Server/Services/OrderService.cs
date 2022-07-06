using Grpc.Core;

namespace gRPC.Sample.Server.Services
{
    public class OrderService : Order.OrderBase
    {
        private readonly ILogger<OrderService> _logger;

        public OrderService(ILogger<OrderService> logger)
        {
            _logger = logger;
        }

        public override Task<OrderResponse> CreateOrder(OrderRequest request, ServerCallContext context)
        {
            var result = new OrderResponse
            {
                OrderId = new Random().Next(1, 999999),
            };
            return Task.FromResult(result);
        }
    }
}
