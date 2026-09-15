using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Catalog.Application.Abstractions;
using GymStore.Modules.Catalog.Domain;

namespace GymStore.Modules.Catalog.Application.Features.GetCategories;

internal sealed class GetCategoriesQueryHandler : IQueryHandler<GetCategoriesQuery, IReadOnlyList<Category>>
{
    private readonly ICategoryRepository _repository;

    public GetCategoriesQueryHandler(ICategoryRepository repository) => _repository = repository;

    public Task<IReadOnlyList<Category>> Handle(GetCategoriesQuery query, CancellationToken ct) =>
        _repository.GetAllAsync(ct);
}
