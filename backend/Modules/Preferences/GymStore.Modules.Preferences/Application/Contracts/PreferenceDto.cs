namespace GymStore.Modules.Preferences.Application.Contracts;

/// <summary>
/// The preference request/response shape. Field names match the pre-refactor Preference entity
/// (and the frontend Preference model). The <c>user</c> navigation is intentionally dropped.
/// </summary>
public sealed class PreferenceDto
{
    public long Id { get; set; }
    public long UserId { get; set; }
    public string Theme { get; set; } = "light";
    public int ItemsPerPage { get; set; } = 12;
    public string SortOrder { get; set; } = "newest";
    public string Currency { get; set; } = "ZAR";
    public string Language { get; set; } = "en";
    public bool Notifications { get; set; } = true;
}

internal static class PreferenceDtoFactory
{
    public static PreferenceDto ToDto(Domain.Preference p) => new()
    {
        Id = p.Id,
        UserId = p.UserId,
        Theme = p.Theme,
        ItemsPerPage = p.ItemsPerPage,
        SortOrder = p.SortOrder,
        Currency = p.Currency,
        Language = p.Language,
        Notifications = p.Notifications
    };
}
