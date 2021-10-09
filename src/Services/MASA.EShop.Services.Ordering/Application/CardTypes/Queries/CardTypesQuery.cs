namespace MASA.EShop.Services.Ordering.Application.CardTypes.Queries
{
    public class CardTypesQuery : Query<IEnumerable<CardType>>
    {
        public override IEnumerable<CardType> Result { get ; set ; }
    }
}
