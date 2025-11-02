namespace LeagueApi.Dtos;

public record MatchRowDto(
    int Id, int CompetitionId, int Year,
    string Home, string Away,
    int HomeGoals, int AwayGoals,
    int HomeCorners, int AwayCorners,
    int HomeShotsTotal, int AwayShotsTotal,
    int HomeShotsOnTarget, int AwayShotsOnTarget,
    int HomeShotsOffTarget, int AwayShotsOffTarget,
    int HomeYellowCards, int AwayYellowCards
);

public record MatchCreateDto(
    int CompetitionId, int Year, int HomeTeamId, int AwayTeamId,
    int HomeGoals, int AwayGoals,
    int HomeCorners, int AwayCorners,
    int HomeShotsTotal, int AwayShotsTotal,
    int HomeShotsOnTarget, int AwayShotsOnTarget,
    int HomeShotsOffTarget, int AwayShotsOffTarget,
    int HomeYellowCards, int AwayYellowCards, DateTime? KickoffUtc
);

public record MatchUpdateDto(
    int HomeGoals, int AwayGoals,
    int HomeCorners, int AwayCorners,
    int HomeShotsTotal, int AwayShotsTotal,
    int HomeShotsOnTarget, int AwayShotsOnTarget,
    int HomeShotsOffTarget, int AwayShotsOffTarget,
    int HomeYellowCards, int AwayYellowCards
);
