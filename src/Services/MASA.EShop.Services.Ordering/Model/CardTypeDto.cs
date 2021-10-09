namespace MASA.EShop.Services.Ordering.Model
{
    public class CardTypeDto
    {
        public int id { get; set; }
        public string name { get; set; } = default!;

        public static CardTypeDto FromCardType(CardType cardType)
        {
            return new CardTypeDto
            {
                id = cardType.Id,
                name = cardType.Name
            };
        }
    }
}
