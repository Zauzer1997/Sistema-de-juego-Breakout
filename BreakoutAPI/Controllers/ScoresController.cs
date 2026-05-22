using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BreakoutAPI.Data;
using BreakoutAPI.Models;

namespace BreakoutAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ScoresController : ControllerBase
{
    private readonly AppDbContext _context;

    public ScoresController(AppDbContext context)
    {
        _context = context;
    }

    // POST: api/scores
    [HttpPost]
    public async Task<IActionResult> SaveScore(Score score)
    {
        /
        var topScores = await _context.Scores
            .OrderByDescending(s => s.ScoreValue)
            .Take(10)
            .ToListAsync();

        
        if (topScores.Count < 10)
        {
            _context.Scores.Add(score);
            await _context.SaveChangesAsync();
            return Ok();
        }

       
        var lowestTopScore = topScores.Last(); 

       
        if (score.ScoreValue <= lowestTopScore.ScoreValue)
        {
            return Ok("Not in top 10");
        }

        
        _context.Scores.Add(score);
        await _context.SaveChangesAsync();

       
        var updatedTop10 = await _context.Scores
            .OrderByDescending(s => s.ScoreValue)
            .Take(10)
            .ToListAsync();

        
        var allScores = await _context.Scores
            .OrderByDescending(s => s.ScoreValue)
            .ToListAsync();

        var toRemove = allScores.Skip(10);

        _context.Scores.RemoveRange(toRemove);
        await _context.SaveChangesAsync();

        return Ok("Inserted into top 10");
    }

    // GET: api/scores/top10
    [HttpGet("top10")]
    public async Task<IActionResult> GetTop10()
    {
        var scores = await _context.Scores
            .OrderByDescending(s => s.ScoreValue)
            .Take(10)
            .ToListAsync();

        return Ok(scores);
    }

    // GET: api/scores/highscore
    [HttpGet("highscore")]
    public async Task<IActionResult> GetHighscore()
    {
        var highscore = await _context.Scores
            .OrderByDescending(s => s.ScoreValue)
            .FirstOrDefaultAsync();

        return Ok(highscore);
    }

}