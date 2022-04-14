namespace Masa.EShop.Services.Ordering.Application.CardTypes.Queries;

public record class CardTypesQuery : Query<IEnumerable<CardType>>
{
    public override IEnumerable<CardType> Result { get; set; } = default!;
}

