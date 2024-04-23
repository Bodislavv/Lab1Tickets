using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using TicketsDomain.Model;
using TicketsInfrastructure;

namespace TicketsInfrastructure.Controllers
{
    public class GamesController : Controller
    {
        private readonly DbticketsContext _context;

        public GamesController(DbticketsContext context)
        {
            _context = context;
        }

        // GET: Games
        public async Task<IActionResult> Index(int? id, string? name)
        {
            if (id == null)
                return RedirectToAction("Tournaments", "Index");

            var tournament = await _context.Tournaments.FindAsync(id);

            if (tournament == null)
                return RedirectToAction("Tournaments", "Index");

            ViewBag.TournamentId = id;
            ViewBag.TournamentName = tournament.Name;

            var gamesByTournament = _context.Games
                                           .Where(g => g.TournamentId == id)
                                           .Include(g => g.TeamANavigation)
                                           .Include(g => g.TeamBNavigation)
                                           .Include(g => g.Tournament)
                                           .Include(g => g.Venue);

            return View(await gamesByTournament.ToListAsync());
        }


        // GET: Games/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.TeamANavigation)
                .Include(g => g.TeamBNavigation)
                .Include(g => g.Tournament)
                .Include(g => g.Venue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // GET: Games/Create
        public IActionResult Create(int tournamentId)
        {
            ViewBag.TeamA = new SelectList(_context.Teams, "Id", "Name");
            ViewBag.TeamB = new SelectList(_context.Teams, "Id", "Name");
            ViewBag.VenueId = new SelectList(_context.Venues, "Id", "Name");

            // Set the TournamentName
            var tournament = _context.Tournaments.Find(tournamentId);
            if (tournament != null)
            {
                ViewBag.TournamentName = tournament.Name;
            }
            else
            {
                ViewBag.TournamentName = "Unknown Tournament";
            }

            ViewBag.TournamentId = tournamentId;

            return View();
        }


        // POST: Games/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(int tournamentId, [Bind("Name,Date,TeamA,VenueId,TournamentId,TeamB,Id")] Game game)
        {
            game.TournamentId = tournamentId;
            if (ModelState.IsValid)
            {
                _context.Add(game);
                await _context.SaveChangesAsync();

                return RedirectToAction("Index", new { id = tournamentId, name = _context.Tournaments.Where(m => m.Id == tournamentId).FirstOrDefault().Name });
            }
            ViewData["TeamA"] = new SelectList(_context.Teams, "Id", "Name", game.TeamA);
            ViewData["TeamB"] = new SelectList(_context.Teams, "Id", "Name", game.TeamB);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name", game.TournamentId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name", game.VenueId);
            return View(game);
        }

        // GET: Games/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.TeamANavigation)
                .Include(g => g.TeamBNavigation)
                .Include(g => g.Tournament)
                .Include(g => g.Venue)
                .FirstOrDefaultAsync(m => m.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            // Retrieve the list of teams from the database
            var teams = await _context.Teams?.ToListAsync();

            if (teams == null)
            {
                // Handle the case where the teams list is null
                // Log an error or throw an exception as appropriate
                // For now, let's just return a generic error view
                return View("Error");
            }

            // Create a SelectList for TeamA and TeamB
            ViewBag.TeamA = new SelectList(teams, "Id", "Name", game.TeamA);
            ViewBag.TeamB = new SelectList(teams, "Id", "Name", game.TeamB);
            ViewBag.VenueId = new SelectList(_context.Venues, "Id", "Name", game.VenueId);
            ViewBag.TournamentId = new SelectList(_context.Tournaments, "Id", "Name", game.TournamentId);

            return View(game);
        }



        // POST: Games/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Name,Date,TeamA,VenueId,TournamentId,TeamB,Id")] Game game)
        {
            if (id != game.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(game);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!GameExists(game.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("Index", new { id = game.TournamentId, name = _context.Tournaments.Where(m => m.Id == game.TournamentId).FirstOrDefault().Name });
            }
            // If we reach here, something went wrong, re-populate the ViewBag and return the view
            ViewData["TeamA"] = new SelectList(_context.Teams, "Id", "Name", game.TeamA);
            ViewData["TeamB"] = new SelectList(_context.Teams, "Id", "Name", game.TeamB);
            ViewData["TournamentId"] = new SelectList(_context.Tournaments, "Id", "Name", game.TournamentId);
            ViewData["VenueId"] = new SelectList(_context.Venues, "Id", "Name", game.VenueId);
            return View(game);
        }


        // GET: Games/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var game = await _context.Games
                .Include(g => g.TeamANavigation)
                .Include(g => g.TeamBNavigation)
                .Include(g => g.Tournament)
                .Include(g => g.Venue)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (game == null)
            {
                return NotFound();
            }

            return View(game);
        }

        // POST: Games/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var game = await _context.Games
                .Include(g => g.Tickets) // Assuming Tickets is the navigation property in Game entity
                .FirstOrDefaultAsync(g => g.Id == id);

            if (game == null)
            {
                return NotFound();
            }

            // Delete associated Tickets first
            _context.Tickets.RemoveRange(game.Tickets);

            // Then delete the Game
            _context.Games.Remove(game);

            await _context.SaveChangesAsync();

            // Redirect to Index action with the necessary parameters
            var tournamentId = game.TournamentId;
            var tournamentName = _context.Tournaments.FirstOrDefault(t => t.Id == tournamentId)?.Name;

            return RedirectToAction(nameof(Index), new { id = tournamentId, name = tournamentName });
        }
        private bool GameExists(int id)
        {
            return _context.Games.Any(e => e.Id == id);
        }
    }
}
