using TicketsDomain.Model;
using TicketsInfrastructure;
namespace TicketsInfrastructure.Services
{
    public class TournamentDataPortServiceFactory : IDataPortServiceFactory<Tournament>
    {
        private readonly DbticketsContext _context;

        public TournamentDataPortServiceFactory(DbticketsContext context)
        {
            _context = context;
        }

        public IImportService<Tournament> GetImportService(string contentType)
        {
            if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new TournamentImportService(_context);
            }
            throw new NotImplementedException($"No import service implemented for tournaments with content type {contentType}");
        }

        public IExportService<Tournament> GetExportService(string contentType)
        {
            if (contentType is "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet")
            {
                return new TournamentExportService(_context);
            }
            throw new NotImplementedException($"No export service implemented for tournaments with content type {contentType}");
        }
    }
}
