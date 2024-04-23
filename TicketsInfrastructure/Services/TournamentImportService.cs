using ClosedXML.Excel;
using TicketsDomain.Model;
using Microsoft.EntityFrameworkCore;
using System;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace TicketsInfrastructure.Services
{
    public class TournamentImportService : IImportService<Tournament>
    {
        private readonly DbticketsContext _context;

        public TournamentImportService(DbticketsContext context)
        {
            _context = context;
        }

        public async Task ImportFromStreamAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanRead)
            {
                throw new ArgumentException("Дані не можуть бути прочитані", nameof(stream));
            }

            using (XLWorkbook workBook = new XLWorkbook(stream))
            {
                foreach (IXLWorksheet worksheet in workBook.Worksheets)
                {
                    var tournamentName = worksheet.Name;
                    var tournament = await _context.Tournaments.FirstOrDefaultAsync(t => t.Name == tournamentName, cancellationToken);
                    if (tournament == null)
                    {
                        tournament = new Tournament
                        {
                            Name = tournamentName,
                            Description = "from EXCEL"
                        };
                        _context.Tournaments.Add(tournament);
                    }

                    foreach (var row in worksheet.RowsUsed().Skip(1))
                    {
                        await AddGameAsync(row, cancellationToken, tournament);
                    }
                }
            }
            await _context.SaveChangesAsync(cancellationToken);
        }

        private async Task AddGameAsync(IXLRow row, CancellationToken cancellationToken, Tournament tournament)
        {
            Game game = new Game();
            game.Name = GetGameName(row);
            game.Date = GetGameDate(row);

            game.TeamA = await GetTeamId(row, 3, cancellationToken);  // TeamA from column 3
            game.TeamB = await GetTeamId(row, 4, cancellationToken);  // TeamB from column 4
            game.Tournament = tournament;
            game.VenueId = await GetVenueId(row, 6, cancellationToken);  // Venue from column 6

            _context.Games.Add(game);
            await _context.SaveChangesAsync(cancellationToken);
        }

        private string GetGameName(IXLRow row)
        {
            return row.Cell(1).Value.ToString();
        }

        private DateTime GetGameDate(IXLRow row)
        {
            return DateTime.Parse(row.Cell(2).Value.ToString());  // Returning DateTime instead of DateOnly
        }


        private async Task<int?> GetTeamId(IXLRow row, int column, CancellationToken cancellationToken)
        {
            var teamName = row.Cell(column).Value.ToString();
            var team = await _context.Teams.FirstOrDefaultAsync(t => t.Name == teamName, cancellationToken);
            return team?.Id;
        }

        private async Task<int?> GetVenueId(IXLRow row, int column, CancellationToken cancellationToken)
        {
            var venueName = row.Cell(column).Value.ToString();
            var venue = await _context.Venues.FirstOrDefaultAsync(v => v.Name == venueName, cancellationToken);
            return venue?.Id;
        }
    }
}
