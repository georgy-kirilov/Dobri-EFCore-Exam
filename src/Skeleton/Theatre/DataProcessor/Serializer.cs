namespace Theatre.DataProcessor
{
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            List<TheatreExportDto> theatres = new List<TheatreExportDto>();

            foreach (var theatre in context.Theatres)
            {
                if (theatre.NumberOfHalls < numbersOfHalls)
                {
                    continue;
                }

                TheatreExportDto exportTheatre = new TheatreExportDto() {
                    Name = theatre.Name,
                    NumberOfHalls = theatre.NumberOfHalls,
                };

                List<TicketExportDto> tickets = new List<TicketExportDto>();
                decimal TotalIncome = 0;

                foreach (var ticket in theatre.Tickets)
                {
                    if (ticket.RowNumber >= 1 && ticket.RowNumber <= 5)
                    {
                        tickets.Add(new TicketExportDto
                        {
                            RowNumber = ticket.RowNumber,
                            Price = ticket.Price
                        }
                        );
                        TotalIncome += ticket.Price;
                    }
                }
                exportTheatre.TotalIncome = TotalIncome;
                tickets.OrderByDescending(t => t.Price);
                exportTheatre.Tickets = (ICollection<Data.Models.Ticket>)tickets;

                theatres.Add(exportTheatre);
            }

            theatres = (List<TheatreExportDto>)theatres.OrderByDescending(t => t.NumberOfHalls).ThenBy(t => t.Name);

            return JsonConvert.SerializeObject(theatres);
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            throw new NotImplementedException();
        }
    }
}
