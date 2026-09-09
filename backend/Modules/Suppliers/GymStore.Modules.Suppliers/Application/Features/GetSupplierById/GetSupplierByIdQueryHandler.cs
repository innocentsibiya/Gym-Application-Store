using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Suppliers.Application.Abstractions;
using GymStore.Modules.Suppliers.Application.Responses;

namespace GymStore.Modules.Suppliers.Application.Features.GetSupplierById;

internal sealed class GetSupplierByIdQueryHandler : IQueryHandler<GetSupplierByIdQuery, SupplierResponse?>
{
    private readonly ISupplierRepository _repository;

    public GetSupplierByIdQueryHandler(ISupplierRepository repository) => _repository = repository;

    public async Task<SupplierResponse?> Handle(GetSupplierByIdQuery query, CancellationToken ct)
    {
        var supplier = await _repository.GetByIdAsync(query.Id, ct);
        return supplier is null ? null : SupplierResponseFactory.ToResponse(supplier);
    }
}
