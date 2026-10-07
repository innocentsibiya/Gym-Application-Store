using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Preferences.Application.Abstractions;
using GymStore.Modules.Preferences.Application.Contracts;
using GymStore.Modules.Preferences.Domain;

namespace GymStore.Modules.Preferences.Application.Features.SavePreferences;

internal sealed class SavePreferencesCommandHandler : ICommandHandler<SavePreferencesCommand, PreferenceDto>
{
    private readonly IPreferenceRepository _repository;

    public SavePreferencesCommandHandler(IPreferenceRepository repository) => _repository = repository;

    public async Task<PreferenceDto> Handle(SavePreferencesCommand command, CancellationToken ct)
    {
        var dto = command.Preferences;

        var prefs = new Preference
        {
            UserId = command.UserId, // the route userId wins, matching the original controller
            Theme = dto.Theme,
            ItemsPerPage = dto.ItemsPerPage,
            SortOrder = dto.SortOrder,
            Currency = dto.Currency,
            Language = dto.Language,
            Notifications = dto.Notifications
        };

        var saved = await _repository.AddOrUpdateAsync(prefs, ct);
        return PreferenceDtoFactory.ToDto(saved);
    }
}
