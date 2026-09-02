using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Catalog.Application.Responses;

namespace GymStore.Modules.Catalog.Application.Features.GetProductById;

public sealed record GetProductByIdQuery(long Id) : IQuery<ProductResponse>;
