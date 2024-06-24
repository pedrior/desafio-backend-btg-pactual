namespace BtgPactual.OrderProcessor.Contracts.Requests;

public sealed record PageRequest
{
    public int Page { get; init; } = 1;
    
    public int Limit { get; init; } = 10;
}