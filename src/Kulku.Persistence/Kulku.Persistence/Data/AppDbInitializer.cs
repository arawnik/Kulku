using Kulku.Domain;
using Kulku.Domain.Ideas;
using Microsoft.EntityFrameworkCore;

namespace Kulku.Persistence.Data;

/// <summary>
/// Provides methods to initialize and seed the application’s database
/// with required default data on startup.
/// </summary>
/// <remarks>
/// This initializer will populate the database with essential seed data such as
/// lookup tables, default configuration values, and any required admin or system users.
/// </remarks>
public static class AppDbInitializer
{
    /// <summary>
    /// Ensures the database schema is up-to-date and seeds initial data.
    /// </summary>
    /// <param name="context">
    /// The <see cref="AppDbContext"/> used to insert seed data.
    /// </param>
    /// <exception cref="DbUpdateException">
    /// Thrown if an error occurs while applying migrations or saving seeded data.
    /// </exception>
    public static void Initialize(AppDbContext context)
    {
        SeedIdeaDomains(context);
        SeedIdeaStatuses(context);
        SeedIdeaPriorities(context);
    }

    private static void SeedIdeaDomains(AppDbContext context)
    {
        if (context.IdeaDomains.Any())
            return;

        context.IdeaDomains.AddRange(
            new IdeaDomain
            {
                Icon = "rocket-takeoff",
                Order = 1,
                Translations =
                [
                    new() { Name = "Career Growth", Language = LanguageCode.English },
                    new() { Name = "Urakehitys", Language = LanguageCode.Finnish },
                ],
            },
            new IdeaDomain
            {
                Icon = "briefcase",
                Order = 2,
                Translations =
                [
                    new() { Name = "Consulting & Business", Language = LanguageCode.English },
                    new() { Name = "Konsultointi ja bisnes", Language = LanguageCode.Finnish },
                ],
            },
            new IdeaDomain
            {
                Icon = "controller",
                Order = 3,
                Translations =
                [
                    new() { Name = "Game Dev", Language = LanguageCode.English },
                    new() { Name = "Pelikehitys", Language = LanguageCode.Finnish },
                ],
            },
            new IdeaDomain
            {
                Icon = "wrench-adjustable",
                Order = 4,
                Translations =
                [
                    new() { Name = "Side Projects", Language = LanguageCode.English },
                    new() { Name = "Sivuprojektit", Language = LanguageCode.Finnish },
                ],
            }
        );

        context.SaveChanges();
    }

    private static void SeedIdeaStatuses(AppDbContext context)
    {
        if (context.IdeaStatuses.Any())
            return;

        context.IdeaStatuses.AddRange(
            new IdeaStatus
            {
                Order = 1,
                Style = "bg-info",
                Translations =
                [
                    new()
                    {
                        Name = "Spark",
                        Description = "Just captured — raw thought.",
                        Language = LanguageCode.English,
                    },
                    new()
                    {
                        Name = "Kipinä",
                        Description = "Juuri tallennettu — jalostamaton ajatus.",
                        Language = LanguageCode.Finnish,
                    },
                ],
            },
            new IdeaStatus
            {
                Order = 2,
                Style = "bg-primary",
                Translations =
                [
                    new()
                    {
                        Name = "Exploring",
                        Description = "Actively researching or fleshing out.",
                        Language = LanguageCode.English,
                    },
                    new()
                    {
                        Name = "Tutkittava",
                        Description = "Aktiivisesti tutkinnassa tai työstössä.",
                        Language = LanguageCode.Finnish,
                    },
                ],
            },
            new IdeaStatus
            {
                Order = 3,
                Style = "bg-success",
                Translations =
                [
                    new()
                    {
                        Name = "Actionable",
                        Description = "Ready to act on.",
                        Language = LanguageCode.English,
                    },
                    new()
                    {
                        Name = "Toteutettava",
                        Description = "Valmis toteutettavaksi.",
                        Language = LanguageCode.Finnish,
                    },
                ],
            },
            new IdeaStatus
            {
                Order = 4,
                Style = "bg-secondary",
                Translations =
                [
                    new()
                    {
                        Name = "Parked",
                        Description = "Not now, revisit later.",
                        Language = LanguageCode.English,
                    },
                    new()
                    {
                        Name = "Pysäköity",
                        Description = "Ei nyt, palataan myöhemmin.",
                        Language = LanguageCode.Finnish,
                    },
                ],
            },
            new IdeaStatus
            {
                Order = 5,
                Style = "bg-dark",
                Translations =
                [
                    new()
                    {
                        Name = "Done",
                        Description = "Completed or promoted elsewhere.",
                        Language = LanguageCode.English,
                    },
                    new()
                    {
                        Name = "Valmis",
                        Description = "Valmis tai edistetään muualla.",
                        Language = LanguageCode.Finnish,
                    },
                ],
            }
        );

        context.SaveChanges();
    }

    private static void SeedIdeaPriorities(AppDbContext context)
    {
        if (context.IdeaPriorities.Any())
            return;

        context.IdeaPriorities.AddRange(
            new IdeaPriority
            {
                Order = 1,
                Style = "bg-light text-dark",
                Translations =
                [
                    new()
                    {
                        Name = "Low",
                        Description = "Nice to have, no urgency.",
                        Language = LanguageCode.English,
                    },
                    new()
                    {
                        Name = "Matala",
                        Description = "Kiva olla, ei kiireellinen.",
                        Language = LanguageCode.Finnish,
                    },
                ],
            },
            new IdeaPriority
            {
                Order = 2,
                Style = "bg-warning text-dark",
                Translations =
                [
                    new()
                    {
                        Name = "Medium",
                        Description = "Worth pursuing when time allows.",
                        Language = LanguageCode.English,
                    },
                    new()
                    {
                        Name = "Keskitaso",
                        Description = "Kannattaa edistää kun aikaa on.",
                        Language = LanguageCode.Finnish,
                    },
                ],
            },
            new IdeaPriority
            {
                Order = 3,
                Style = "bg-danger",
                Translations =
                [
                    new()
                    {
                        Name = "High",
                        Description = "Important, act on soon.",
                        Language = LanguageCode.English,
                    },
                    new()
                    {
                        Name = "Korkea",
                        Description = "Tärkeä, toimi pian.",
                        Language = LanguageCode.Finnish,
                    },
                ],
            }
        );

        context.SaveChanges();
    }
}
