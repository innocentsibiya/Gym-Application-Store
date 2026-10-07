namespace GymStore.Modules.Preferences.Domain;

/// <summary>A user's UI/shopping preferences, owned by the Preferences module (references the user by id).</summary>
public class Preference
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
