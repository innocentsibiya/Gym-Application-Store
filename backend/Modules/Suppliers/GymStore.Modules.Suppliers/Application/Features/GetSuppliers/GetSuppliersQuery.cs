using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Suppliers.Application.Responses;

namespace GymStore.Modules.Suppliers.Application.Features.GetSuppliers;

public sealed record GetSuppliersQuery : IQuery<IReadOnlyList<SupplierResponse>>;
