namespace DandyDotnet.Samples.OnlineShop.Api.SharedKernel.Domain.Abstractions;

public static class SoftDeletedAtExtensions
{
    public static void Delete(this ISoftDeletedAt softDeletedAt)
    {
        softDeletedAt.Deleted = true;
        softDeletedAt.DeletedAt = DateTime.UtcNow;
    }
}