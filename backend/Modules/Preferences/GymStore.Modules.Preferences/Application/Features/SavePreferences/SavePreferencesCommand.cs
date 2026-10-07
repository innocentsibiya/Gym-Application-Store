using GymStore.BuildingBlocks.Cqrs;
using GymStore.Modules.Preferences.Application.Contracts;

namespace GymStore.Modules.Preferences.Application.Features.SavePreferences;

/// <summary>Creates or updates the given user's preferences and returns the saved values.</summary>
public sealed record SavePreferencesCommand(long UserId, PreferenceDto Preferences) : ICommand<PreferenceDto>;
