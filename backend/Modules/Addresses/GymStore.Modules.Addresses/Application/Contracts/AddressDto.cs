namespace GymStore.Modules.Addresses.Application.Contracts;

/// <summary>
/// The address request/response shape. Property names/casing are identical to the pre-refactor
/// AddressDto so the API contract (consumed by the frontend) is unchanged.
/// </summary>
public sealed class AddressDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Street { get; set; } = string.Empty;
    public string City { get; set; } = string.Empty;
    public string Province { get; set; } = string.Empty;
    public string PostalCode { get; set; } = string.Empty;
    public string Country { get; set; } = string.Empty;
    public string AddressType { get; set; } = "Shipping";
    public bool IsDefault { get; set; }
}

internal static class AddressDtoFactory
{
    public static AddressDto ToDto(Domain.Address a) => new()
    {
        Id = a.Id,
        UserId = a.UserId,
        Street = a.Street,
        City = a.City,
        Province = a.Province,
        PostalCode = a.PostalCode,
        Country = a.Country,
        AddressType = a.AddressType,
        IsDefault = a.IsDefault
    };
}
