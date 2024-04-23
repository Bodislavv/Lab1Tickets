using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using TicketsDomain.Model;
using TicketsInfrastructure;

namespace TicketsMVC.WebMVC.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ChartsController : ControllerBase
    {
        private readonly DbticketsContext _dbticketsContext;

        public ChartsController(DbticketsContext dbticketsContext)
        {
            _dbticketsContext = dbticketsContext;
        }

        [HttpGet("teamCountByCountry")]
        public async Task<IActionResult> GetTeamCountByCountryAsync()
        {
            var teamCounts = await _dbticketsContext.Teams
                .Include(t => t.Country)
                .GroupBy(t => t.Country.Name)
                .Select(g => new { Country = g.Key, Count = g.Count() })
                .ToListAsync();

            return Ok(teamCounts);
        }
        [HttpGet("gameCountByMonth")]
        public async Task<IActionResult> GetGameCountByMonthAsync()
        {
            var gameCounts = await _dbticketsContext.Games
                .GroupBy(g => new { g.Date.Year, g.Date.Month })
                .Select(g => new
                {
                    Month = $"{GetMonthName(g.Key.Month)} {g.Key.Year}",
                    Count = g.Count()
                })
                .ToListAsync();

            return Ok(gameCounts);
        }
        private static string GetMonthName(int month)
        {
            switch (month)
            {
                case 1: return "January";
                case 2: return "February";
                case 3: return "March";
                case 4: return "April";
                case 5: return "May";
                case 6: return "June";
                case 7: return "July";
                case 8: return "August";
                case 9: return "September";
                case 10: return "October";
                case 11: return "November";
                case 12: return "December";
                default: return "";
            }
        }

    }
}