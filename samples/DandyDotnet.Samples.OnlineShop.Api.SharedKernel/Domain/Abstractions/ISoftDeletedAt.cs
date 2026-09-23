namespace DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Domain.Abstractions;

public interface ISoftDeletedAt
{
    bool Deleted { get; set; }
    DateTime? DeletedAt { get; set; }
}