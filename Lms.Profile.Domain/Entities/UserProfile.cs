namespace Lms.Profile.Domain.Entities;

// UserProfile representerar en användares profil i databasen.
// Klassen används av Entity Framework Core för att skapa och hantera tabellen UserProfiles.
//
// AI användes som stöd för att förstå entity-strukturen och relationen mellan
// databasmodell och DTO:er. Modellen anpassades därefter manuellt efter projektets krav.

public class UserProfile
{
    // Primärnyckel för profilen.
    public Guid Id { get; set; } = Guid.NewGuid();

    // ID från Auth API som identifierar användaren.
    public string UserId { get; set; } = null!;

    // Användarens förnamn.
    public string FirstName { get; set; } = null!;

    // Användarens efternamn.
    public string LastName { get; set; } = null!;

    // Användarens e-postadress.
    public string Email { get; set; } = null!;

    // Valfri beskrivning eller presentation av användaren.
    public string? Bio { get; set; }

    // URL till användarens profilbild.
    public string? ProfileImageUrl { get; set; }

    // Roll i LMS-systemet.
    // Standardvärdet är Student.
    public string Role { get; set; } = "Student";

    // Tidpunkt då profilen skapades.
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
}