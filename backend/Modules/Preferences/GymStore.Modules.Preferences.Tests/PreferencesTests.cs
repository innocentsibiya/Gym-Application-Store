using GymStore.Modules.Preferences.Application.Contracts;
using GymStore.Modules.Preferences.Application.Features.GetPreferences;
using GymStore.Modules.Preferences.Application.Features.SavePreferences;
using GymStore.Modules.Preferences.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace GymStore.Modules.Preferences.Tests;

public class PreferencesTests
{
    private static PreferencesDbContext NewContext() =>
        new(new DbContextOptionsBuilder<PreferencesDbContext>()
            .UseInMemoryDatabase($"preferences-tests-{Guid.NewGuid()}")
            .Options);

    [Fact]
    public async Task GetPreferences_WhenNone_ReturnsUnsavedDefaults()
    {
        await using var ctx = NewContext();
        var handler = new GetPreferencesQueryHandler(new PreferenceRepository(ctx));

        var result = await handler.Handle(new GetPreferencesQuery(1), CancellationToken.None);

        Assert.Equal(1, result.UserId);
        Assert.Equal("light", result.Theme);
        Assert.Equal(12, result.ItemsPerPage);
        Assert.Equal("ZAR", result.Currency);
        Assert.Empty(await ctx.Preferences.ToListAsync()); // defaults are not persisted
    }

    [Fact]
    public async Task Save_Then_Get_PersistsAndReturns()
    {
        await using var ctx = NewContext();
        var repo = new PreferenceRepository(ctx);
        var save = new SavePreferencesCommandHandler(repo);

        var dto = new PreferenceDto { Theme = "dark", ItemsPerPage = 24, Currency = "USD", Notifications = false };
        var saved = await save.Handle(new SavePreferencesCommand(1, dto), CancellationToken.None);

        Assert.Equal(1, saved.UserId);
        Assert.Equal("dark", saved.Theme);
        Assert.True(saved.Id > 0);

        var fetched = await new GetPreferencesQueryHandler(repo).Handle(new GetPreferencesQuery(1), CancellationToken.None);
        Assert.Equal("dark", fetched.Theme);
        Assert.Equal(24, fetched.ItemsPerPage);
        Assert.False(fetched.Notifications);
    }

    [Fact]
    public async Task Save_Twice_UpdatesInPlace()
    {
        await using var ctx = NewContext();
        var repo = new PreferenceRepository(ctx);
        var save = new SavePreferencesCommandHandler(repo);

        await save.Handle(new SavePreferencesCommand(1, new PreferenceDto { Theme = "dark" }), CancellationToken.None);
        await save.Handle(new SavePreferencesCommand(1, new PreferenceDto { Theme = "light", Language = "af" }), CancellationToken.None);

        var row = await ctx.Preferences.SingleAsync(); // one row per user, updated in place
        Assert.Equal("light", row.Theme);
        Assert.Equal("af", row.Language);
    }
}
