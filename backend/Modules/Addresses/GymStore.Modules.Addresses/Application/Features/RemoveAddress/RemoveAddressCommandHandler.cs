using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Addresses.Application.Abstractions;

namespace GymStore.Modules.Addresses.Application.Features.RemoveAddress;

internal sealed class RemoveAddressCommandHandler : ICommandHandler<RemoveAddressCommand, bool>
{
    private readonly IAddressRepository _repository;
    private readonly IAddressCache _cache;

    public RemoveAddressCommandHandler(IAddressRepository repository, IAddressCache cache)
    {
        _repository = repository;
        _cache = cache;
    }

    public async Task<bool> Handle(RemoveAddressCommand command, CancellationToken ct)
    {
        var address = await _repository.GetByIdAsync(command.Id, ct);
        if (address is null)
        {
            return false;
        }

        _repository.Remove(address);
        await _repository.SaveChangesAsync(ct);
        await _cache.RemoveAsync(address.UserId, ct);
        return true;
    }
}
