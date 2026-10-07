using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Preferences.Application.Abstractions;
using GymStore.Modules.Preferences.Application.Contracts;

namespace GymStore.Modules.Preferences.Application.Features.GetPreferences;

/// <summary>Returns the user's preferences, or unsaved defaults when none exist (matches the original).</summary>
internal sealed class GetPreferencesQueryHandler : IQueryHandler<GetPreferencesQuery, PreferenceDto>
{
    private readonly IPreferenceRepository _repository;

    public GetPreferencesQueryHandler(IPreferenceRepository repository) => _repository = repository;

    public async Task<PreferenceDto> Handle(GetPreferencesQuery query, CancellationToken ct)
    {
        var prefs = await _repository.GetByUserIdAsync(query.UserId, ct);
        return prefs is null
            ? new PreferenceDto { UserId = query.UserId }   // defaults, not persisted
            : PreferenceDtoFactory.ToDto(prefs);
    }
}
