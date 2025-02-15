using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using StudentFunctions.Models.Game;

public class FifaService
{
    private readonly FifaContext _context;

    public FifaService(FifaContext context)
    {
        _context = context;
    }

    public async Task<List<StudentFunctions.Models.Game.Game>> GetAllGames()
    {
        return await _context.Games.ToListAsync();
    }
}
