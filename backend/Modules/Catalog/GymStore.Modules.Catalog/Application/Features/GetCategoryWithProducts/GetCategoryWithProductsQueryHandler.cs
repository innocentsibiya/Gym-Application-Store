using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Catalog.Application.Abstractions;
using GymStore.Modules.Catalog.Domain;

namespace GymStore.Modules.Catalog.Application.Features.GetCategoryWithProducts;

internal sealed class GetCategoryWithProductsQueryHandler : IQueryHandler<GetCategoryWithProductsQuery, Category?>
{
    private readonly ICategoryRepository _repository;

    public GetCategoryWithProductsQueryHandler(ICategoryRepository repository) => _repository = repository;

    public Task<Category?> Handle(GetCategoryWithProductsQuery query, CancellationToken ct) =>
        _repository.GetWithProductsAsync(query.Id, ct);
}
