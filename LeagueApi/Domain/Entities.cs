using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace LeagueApi.Domain;

// ✅ Team: solo Id e Nome
public class Team
{
    public int Id { get; set; }
    [MaxLength(80)] public string Name { get; set; } = "";
}

// ✅ Competition: lasciamo com'era (Id + Nome)
public class Competition
{
    public int Id { get; set; }
    [MaxLength(80)] public string Name { get; set; } = "";
}

// ✅ Match minimale con anno, squadre e statistiche richieste
public class Match
{
    public int Id { get; set; }
    public int CompetitionId { get; set; }
    public Competition Competition { get; set; } = null!;
    public int Year { get; set; }

    public int HomeId { get; set; }
    public Team Home { get; set; } = null!;
    public int AwayId { get; set; }
    public Team Away { get; set; } = null!;

    [Range(0, 99)] public int? HomeGoals { get; set; }
    [Range(0, 99)] public int? AwayGoals { get; set; }

    [Range(0, 99)] public int? HomeCorners { get; set; }
    [Range(0, 99)] public int? AwayCorners { get; set; }

    [Range(0, 200)] public int? HomeShotsTotal { get; set; }
    [Range(0, 200)] public int? AwayShotsTotal { get; set; }
    [Range(0, 200)] public int? HomeShotsOnTarget { get; set; }
    [Range(0, 200)] public int? AwayShotsOnTarget { get; set; }
    [Range(0, 200)] public int? HomeShotsOffTarget { get; set; }
    [Range(0, 200)] public int? AwayShotsOffTarget { get; set; }

    [Range(0, 99)] public int? HomeYellowCards { get; set; }
    [Range(0, 99)] public int? AwayYellowCards { get; set; }

    // ✅ nuova proprietà
    public DateTime? KickoffUtc { get; set; }
}

