using GymStore.Modules.Suppliers.Domain;

namespace GymStore.Modules.Suppliers.Application.Responses;

/// <summary>
/// HTTP response for a supplier. Carries supplier fields only — the pre-refactor endpoint also
/// embedded each supplier's inventory (and products); that cross-module data is dropped.
/// </summary>
public sealed class SupplierResponse
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? ContactName { get; set; }
    public string? Phone { get; set; }
    public string? Email { get; set; }
    public string? Address { get; set; }
    public DateTime CreatedAt { get; set; }
}

internal static class SupplierResponseFactory
{
    public static SupplierResponse ToResponse(Supplier s) => new()
    {
        Id = s.Id,
        Name = s.Name,
        ContactName = s.ContactName,
        Phone = s.Phone,
        Email = s.Email,
        Address = s.Address,
        CreatedAt = s.CreatedAt
    };
}
