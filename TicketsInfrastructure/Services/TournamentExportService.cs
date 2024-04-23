using ClosedXML.Excel;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using TicketsDomain.Model;

namespace TicketsInfrastructure.Services
{
    public class TournamentExportService : IExportService<Tournament>
    {
        private const string RootWorksheetName = "";

        private static readonly IReadOnlyList<string> HeaderNames =
            new string[]
            {
                "Name",
                "Date",
                "Home team",
                "Away team",
                "Tournament",
                "Venue"
            };

        private readonly DbticketsContext _context;

        private static void WriteHeader(IXLWorksheet worksheet)
        {
            for (int columnIndex = 0; columnIndex < HeaderNames.Count; columnIndex++)
            {
                worksheet.Cell(1, columnIndex + 1).Value = HeaderNames[columnIndex];
            }
            worksheet.Row(1).Style.Font.Bold = true;
        }

        private void WriteGame(IXLWorksheet worksheet, Game game, int rowIndex)
        {
            worksheet.Cell(rowIndex, 1).Value = game.Name;
            worksheet.Cell(rowIndex, 2).Value = game.Date.ToString("yyyy-MM-dd");
            worksheet.Cell(rowIndex, 3).Value = game.TeamANavigation?.Name;
            worksheet.Cell(rowIndex, 4).Value = game.TeamBNavigation?.Name;
            worksheet.Cell(rowIndex, 5).Value = game.Tournament?.Name;
            worksheet.Cell(rowIndex, 6).Value = game.Venue?.Name;
        }

        private void WriteGames(IXLWorksheet worksheet, ICollection<Game> games)
        {
            WriteHeader(worksheet);
            int rowIndex = 2;
            foreach (var game in games)
            {
                WriteGame(worksheet, game, rowIndex);
                rowIndex++;
            }
        }

        public TournamentExportService(DbticketsContext context)
        {
            _context = context;
        }

        public async Task WriteToAsync(Stream stream, CancellationToken cancellationToken)
        {
            if (!stream.CanWrite)
            {
                throw new ArgumentException("Input stream is not writable");
            }

            var tournaments = await _context.Tournaments
                .ToListAsync(cancellationToken);

            var games = await _context.Games
                .Include(game => game.TeamANavigation)
                .Include(game => game.TeamBNavigation)
                .Include(game => game.Tournament)
                .Include(game => game.Venue)
                .ToListAsync(cancellationToken);

            var workbook = new XLWorkbook();

            foreach (var tournament in tournaments)
            {
                if (tournament != null)
                {
                    var tournamentGames = games.Where(game => game.TournamentId == tournament.Id).ToList();
                    var worksheet = workbook.Worksheets.Add(tournament.Name ?? "Unnamed");
                    WriteGames(worksheet, tournamentGames);
                }
            }

            workbook.SaveAs(stream);
        }
    }
}
