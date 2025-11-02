using LeagueApi.Data;
using LeagueApi.Domain;
using LeagueApi.Dtos;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeagueApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MatchesController : ControllerBase
{
    private readonly AppDbContext _db;

    public MatchesController(AppDbContext db) => _db = db;

    // GET /api/matches?competitionId=&year=&page=1&pageSize=20
    [HttpGet]
    public async Task<PagedResult<MatchRowDto>> List(
        [FromQuery] int? competitionId,
        [FromQuery] int? year,
        [FromQuery] int page = 1,
        [FromQuery] int pageSize = 20)
    {
        if (page < 1) page = 1;
        if (pageSize is < 1 or > 200) pageSize = 20;

        var q = _db.Matches
            .AsNoTracking()
            .Include(m => m.Home)
            .Include(m => m.Away)
            .AsQueryable();

        if (competitionId is not null) q = q.Where(m => m.CompetitionId == competitionId);
        if (year is not null) q = q.Where(m => m.Year == year);

        var total = await q.CountAsync();

        var items = await q.OrderBy(m => m.Year).ThenBy(m => m.Id)
            .Skip((page - 1) * pageSize)
            .Take(pageSize)
            .Select(m => new MatchRowDto(
                m.Id, m.CompetitionId, m.Year,
                m.Home.Name, m.Away.Name,
                m.HomeGoals ?? 0, m.AwayGoals ?? 0,
                m.HomeCorners ?? 0, m.AwayCorners ?? 0,
                m.HomeShotsTotal ?? 0, m.AwayShotsTotal ?? 0,
                m.HomeShotsOnTarget ?? 0, m.AwayShotsOnTarget ?? 0,
                m.HomeShotsOffTarget ?? 0, m.AwayShotsOffTarget ?? 0,
                m.HomeYellowCards ?? 0, m.AwayYellowCards ?? 0
            ))
            .ToListAsync();

        return new(items, total, page, pageSize);
    }

    // GET /api/matches/{id}
    [HttpGet("{id:int}")]
    public async Task<ActionResult<MatchRowDto>> GetOne(int id)
    {
        var m = await _db.Matches
            .AsNoTracking()
            .Include(x => x.Home)
            .Include(x => x.Away)
            .FirstOrDefaultAsync(x => x.Id == id);

        if (m is null) return NotFound();
        return MapToDto(m);
    }

    // POST /api/matches
    [HttpPost]
    public async Task<ActionResult<MatchRowDto>> Create([FromBody] MatchCreateDto dto)
    {
        if (dto.HomeTeamId == dto.AwayTeamId)
            return BadRequest("HomeTeamId e AwayTeamId non possono coincidere.");

        var home = await _db.Teams.FindAsync(dto.HomeTeamId);
        var away = await _db.Teams.FindAsync(dto.AwayTeamId);
        if (home is null || away is null) return BadRequest("Team non trovato.");

        var m = new Match
        {
            CompetitionId = dto.CompetitionId,
            Year = dto.Year,
            HomeId = dto.HomeTeamId,
            AwayId = dto.AwayTeamId,
            HomeGoals = dto.HomeGoals, AwayGoals = dto.AwayGoals,
            HomeCorners = dto.HomeCorners, AwayCorners = dto.AwayCorners,
            HomeShotsTotal = dto.HomeShotsTotal, AwayShotsTotal = dto.AwayShotsTotal,
            HomeShotsOnTarget = dto.HomeShotsOnTarget, AwayShotsOnTarget = dto.AwayShotsOnTarget,
            HomeShotsOffTarget = dto.HomeShotsOffTarget, AwayShotsOffTarget = dto.AwayShotsOffTarget,
            HomeYellowCards = dto.HomeYellowCards, AwayYellowCards = dto.AwayYellowCards,
            KickoffUtc = dto.KickoffUtc
        };

        _db.Matches.Add(m);
        await _db.SaveChangesAsync();

        // carico le navigation per il DTO
        await _db.Entry(m).Reference(x => x.Home).LoadAsync();
        await _db.Entry(m).Reference(x => x.Away).LoadAsync();

        var dtoOut = MapToDto(m);
        return CreatedAtAction(nameof(GetOne), new { id = m.Id }, dtoOut);
    }

    // PUT /api/matches/{id}
    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, [FromBody] MatchUpdateDto dto)
    {
        var m = await _db.Matches.FindAsync(id);
        if (m is null) return NotFound();

        m.HomeGoals = dto.HomeGoals; m.AwayGoals = dto.AwayGoals;
        m.HomeCorners = dto.HomeCorners; m.AwayCorners = dto.AwayCorners;
        m.HomeShotsTotal = dto.HomeShotsTotal; m.AwayShotsTotal = dto.AwayShotsTotal;
        m.HomeShotsOnTarget = dto.HomeShotsOnTarget; m.AwayShotsOnTarget = dto.AwayShotsOnTarget;
        m.HomeShotsOffTarget = dto.HomeShotsOffTarget; m.AwayShotsOffTarget = dto.AwayShotsOffTarget;
        m.HomeYellowCards = dto.HomeYellowCards; m.AwayYellowCards = dto.AwayYellowCards;

        await _db.SaveChangesAsync();
        return NoContent();
    }

    // DELETE /api/matches/{id}
    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var m = await _db.Matches.FindAsync(id);
        if (m is null) return NotFound();

        _db.Matches.Remove(m);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // ---- helper ----
    private static MatchRowDto MapToDto(Match m) =>
        new(
            m.Id, m.CompetitionId, m.Year,
            m.Home?.Name ?? "", m.Away?.Name ?? "",
            m.HomeGoals ?? 0, m.AwayGoals ?? 0,
            m.HomeCorners ?? 0, m.AwayCorners ?? 0,
            m.HomeShotsTotal ?? 0, m.AwayShotsTotal ?? 0,
            m.HomeShotsOnTarget ?? 0, m.AwayShotsOnTarget ?? 0,
            m.HomeShotsOffTarget ?? 0, m.AwayShotsOffTarget ?? 0,
            m.HomeYellowCards ?? 0, m.AwayYellowCards ?? 0
        );
}
