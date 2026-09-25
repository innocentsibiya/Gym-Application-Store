using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Suppliers.Application.Abstractions;
using GymStore.Modules.Suppliers.Application.Responses;

namespace GymStore.Modules.Suppliers.Application.Features.GetSuppliers;

internal sealed class GetSuppliersQueryHandler : IQueryHandler<GetSuppliersQuery, IReadOnlyList<SupplierResponse>>
{
    private readonly ISupplierRepository _repository;

    public GetSuppliersQueryHandler(ISupplierRepository repository) => _repository = repository;

    public async Task<IReadOnlyList<SupplierResponse>> Handle(GetSuppliersQuery query, CancellationToken ct)
    {
        var suppliers = await _repository.GetAllAsync(ct);
        return suppliers.Select(SupplierResponseFactory.ToResponse).ToList();
    }
}
