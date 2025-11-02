using LeagueApi.Data;
using LeagueApi.Domain;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace LeagueApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TeamsController(AppDbContext db) : ControllerBase
{
    [HttpGet] public Task<List<Team>> Get() => db.Teams.OrderBy(t => t.Name).ToListAsync();

    [HttpGet("{id:int}")]
    public async Task<ActionResult<Team>> GetOne(int id) =>
        await db.Teams.FindAsync(id) is { } t ? t : NotFound();

    [HttpPost]
    public async Task<ActionResult<Team>> Create(Team t)
    {
        db.Teams.Add(t); await db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetOne), new { id = t.Id }, t);
    }

    [HttpPut("{id:int}")]
    public async Task<IActionResult> Update(int id, Team input)
    {
        if (id != input.Id) return BadRequest();
        db.Entry(input).State = EntityState.Modified;
        await db.SaveChangesAsync(); return NoContent();
    }

    [HttpDelete("{id:int}")]
    public async Task<IActionResult> Delete(int id)
    {
        var t = await db.Teams.FindAsync(id); if (t is null) return NotFound();
        db.Teams.Remove(t); await db.SaveChangesAsync(); return NoContent();
    }
}
