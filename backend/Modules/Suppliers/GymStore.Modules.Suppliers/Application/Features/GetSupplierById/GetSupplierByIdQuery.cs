using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Suppliers.Application.Responses;

namespace GymStore.Modules.Suppliers.Application.Features.GetSupplierById;

public sealed record GetSupplierByIdQuery(long Id) : IQuery<SupplierResponse?>;
