namespace MASA.EShop.Services.Ordering.Application.CardTypes
{
    public class CardTypesQueryHandler
    {
        private readonly IOrderRepository _orderRepository;

        public CardTypesQueryHandler(IOrderRepository orderRepository)
        {
            _orderRepository = orderRepository;
        }

        [EventHandler]
        public async Task OrderQueryAsync(CardTypesQuery query)
        {
            query.Result = await _orderRepository.GetCardTypesAsync();
        }
    }
}
