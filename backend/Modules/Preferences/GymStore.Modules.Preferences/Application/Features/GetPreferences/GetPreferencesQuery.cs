using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Preferences.Application.Contracts;

namespace GymStore.Modules.Preferences.Application.Features.GetPreferences;

public sealed record GetPreferencesQuery(long UserId) : IQuery<PreferenceDto>;
