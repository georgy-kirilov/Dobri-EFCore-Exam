namespace Theatre.DataProcessor
{
    using Microsoft.EntityFrameworkCore;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.DataProcessor.ExportDto;

    public class Serializer
    {
        public static string ExportTheatres(TheatreContext context, int numbersOfHalls)
        {
            var theatres = context.Theatres
                .ToList()
                .Where(th => th.NumberOfHalls >= numbersOfHalls && th.Tickets.Count >= 20)
                .Select(th => new TheatreExportDto
                {
                    Name = th.Name,
                    Halls = th.NumberOfHalls,

                    TotalIncome = th.Tickets.ToList()
                        .Where(ti => ti.RowNumber >= 1 && ti.RowNumber <= 5).Sum(ti => ti.Price),

                    Tickets = th.Tickets.ToList()
                        .Where(ti => ti.RowNumber >= 1 && ti.RowNumber <= 5)
                        .Select(ti => new TicketExportDto
                        {
                            Price = ti.Price,
                            RowNumber = ti.RowNumber
                        })
                        .OrderByDescending(ti => ti.Price)
                        .ToList()
                })
                .OrderByDescending(th => th.Halls)
                .ThenBy(th => th.Name);

            return JsonConvert.SerializeObject(theatres, Formatting.Indented, new DecimalFormatConverter());
            /*
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

                var tickets = new HashSet<TicketExportDto>();
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

            theatres = theatres.OrderByDescending(t => t.NumberOfHalls).ThenBy(t => t.Name).ToList();

            return JsonConvert.SerializeObject(theatres);*/
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            List<PlaysExportDto> plays = new List<PlaysExportDto>();

            foreach (var play in context.Plays)
            {

                if (play.Rating > rating)
                {
                    continue;
                }

             PlaysExportDto exportPlay = new PlaysExportDto()
             { 
                Title = play.Title,
                Duration = play.Duration,
                Rating = play.Rating,
                Genre = play.Genre
             };
                List<CastExportDto> casts = new List<CastExportDto>();
                foreach (var cast in play.Casts)
                {
                    if (cast.IsMainCharacter)
                    {
                        casts.Add(new CastExportDto
                        {
                            FullName = cast.FullName,
                            MainCharacter = "Plays main character in '" + play.Title + "'." 
                        });
                    }
                }
                casts = casts.OrderByDescending(c => c.FullName).ToList();
                exportPlay.Casts = (ICollection<Data.Models.Cast>)casts;
                plays.Add(exportPlay);
            }
            plays = plays.OrderBy(p => p.Title).ThenByDescending(p => p.Genre).ToList();

            XmlRootAttribute root = new XmlRootAttribute("Plays");
            XmlSerializer serializer = new XmlSerializer(typeof(PlaysExportDto[]), root);
            StringBuilder sb = new StringBuilder();

            using (StringWriter sw = new StringWriter(sb))
            {
                serializer.Serialize(sw, plays);
            }
            return sb.ToString();
        }
    }

    public class DecimalFormatConverter : JsonConverter
    {
        public override bool CanConvert(Type objectType)
        {
            return (objectType == typeof(decimal));
        }

        public override void WriteJson(JsonWriter writer, object value,
                                       JsonSerializer serializer)
        {
            writer.WriteValue(string.Format("{0:N2}", value));
        }

        public override bool CanRead
        {
            get { return false; }
        }

        public override object ReadJson(JsonReader reader, Type objectType,
                                     object existingValue, JsonSerializer serializer)
        {
            throw new NotImplementedException();
        }
    }
}
