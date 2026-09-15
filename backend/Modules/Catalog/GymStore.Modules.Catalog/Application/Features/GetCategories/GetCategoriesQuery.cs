using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Catalog.Domain;

namespace GymStore.Modules.Catalog.Application.Features.GetCategories;

public sealed record GetCategoriesQuery : IQuery<IReadOnlyList<Category>>;
