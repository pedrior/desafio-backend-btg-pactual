using System.ComponentModel.DataAnnotations;

namespace BtgPactual.OrderProcessor.Options;

public sealed record RabbitMqOptions
{
    public const string SectionName = "Mq";

    [Required]
    public string Host { get; init; } = null!;
    
    [Required]
    public string Username { get; init; } = null!;
    
    [Required]
    public string Password { get; init; } = null!;
}