namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.IO;
    using System.Linq;
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
        }

        public static string ExportPlays(TheatreContext context, double rating)
        {
            var plays = context.Plays.ToList().Where(pl => pl.Rating <= rating)
                .Select(pl => new PlayExportDto
                {
                    Title = pl.Title,
                    Duration = pl.Duration.ToString("c"),
                    Rating = pl.Rating == 0 ? "Premier" : pl.Rating.ToString(),
                    Genre = pl.Genre,
                    Actors = pl.Casts.Where(c => c.IsMainCharacter).Select(c => new ActorExportDto
                    {
                        FullName = c.FullName,
                        MainCharacter = $"Plays main character in '{pl.Title}'."
                    })
                    .OrderByDescending(a => a.FullName).ToArray(),
                })
                .OrderBy(pl => pl.Title)
                .ThenByDescending(pl => pl.Genre).ToArray();

            XmlRootAttribute root = new XmlRootAttribute("Plays");
            XmlSerializer serializer = new XmlSerializer(typeof(PlayExportDto[]), root);
            var stringWriter = new StringWriter();
            serializer.Serialize(stringWriter, plays);
            return stringWriter.ToString();
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
