using GymStore.Modules.Addresses.Application.Contracts;

namespace GymStore.Modules.Addresses.Application.Abstractions;

/// <summary>Cache-aside store for a user's address list.</summary>
public interface IAddressCache
{
    Task<IReadOnlyList<AddressDto>?> GetAsync(long userId, CancellationToken ct);
    Task SetAsync(long userId, IReadOnlyList<AddressDto> addresses, CancellationToken ct);
    Task RemoveAsync(long userId, CancellationToken ct);
}
