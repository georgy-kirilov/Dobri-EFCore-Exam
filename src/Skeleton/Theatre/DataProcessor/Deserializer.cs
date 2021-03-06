namespace Theatre.DataProcessor
{
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
    using System.Linq;
    using System.Text;
    using System.Xml.Serialization;
    using Theatre.Data;
    using Theatre.Data.Models;
    using Theatre.Data.Models.Enums;
    using Theatre.DataProcessor.ImportDto;

    public class Deserializer
    {
        private const string ErrorMessage = "Invalid data!";

        private const string SuccessfulImportPlay
            = "Successfully imported {0} with genre {1} and a rating of {2}!";

        private const string SuccessfulImportActor
            = "Successfully imported actor {0} as a {1} character!";

        private const string SuccessfulImportTheatre
            = "Successfully imported theatre {0} with #{1} tickets!";

        public static string ImportPlays(TheatreContext context, string xmlString)
        {
            var messageBuilder = new StringBuilder();

            Type type = typeof(PlayDto[]);

            var root = new XmlRootAttribute("Plays");
            var serializer = new XmlSerializer(type, root);

            using var stringReader = new StringReader(xmlString);
            var playDtos = (PlayDto[])serializer.Deserialize(stringReader);

            foreach (PlayDto playDto in playDtos)
            {
                bool isDurationValid = TimeSpan.TryParseExact(
                    playDto.Duration, "c", CultureInfo.InvariantCulture, out TimeSpan duration);

                bool isGenreValid = Enum.IsDefined(typeof(Genre), playDto.Genre);

                bool isDurationRangeValid = duration.CompareTo(new TimeSpan(1, 0, 0)) > 0;

                if (!IsValid(playDto) || !isDurationValid || !isGenreValid || !isDurationRangeValid)
                {
                    messageBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                var play = new Play
                {
                    Title = playDto.Title,
                    Description = playDto.Description,
                    Screenwriter = playDto.Screenwriter,
                    Rating = playDto.Rating,
                    Duration = duration,
                    Genre = (Genre)Enum.Parse(typeof(Genre), playDto.Genre),
                };

                messageBuilder.AppendLine(
                    string.Format(SuccessfulImportPlay, play.Title, play.Genre, play.Rating));

                context.Add(play);
                context.SaveChanges();
            }

            return messageBuilder.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            var messageBuilder = new StringBuilder();
            var serializer = new XmlSerializer(typeof(CastDto[]), new XmlRootAttribute("Casts"));
            var castDtos = (CastDto[])serializer.Deserialize(new StringReader(xmlString));

            foreach (CastDto castDto in castDtos)
            {
                if (!IsValid(castDto))
                {
                    messageBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                var cast = new Cast
                {
                    FullName = castDto.FullName,
                    PhoneNumber = castDto.PhoneNumber,
                    IsMainCharacter = castDto.IsMainCharacter,
                    PlayId = castDto.PlayId,
                };

                string roleType = cast.IsMainCharacter ? "main" : "lesser";

                messageBuilder.AppendLine(
                    string.Format(
                        SuccessfulImportActor, cast.FullName, roleType));

                context.Add(cast);
                context.SaveChanges();
            }

            return messageBuilder.ToString().TrimEnd();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            var messageBuilder = new StringBuilder();
            var theatreDtos = JsonConvert.DeserializeObject<TheatreImportDto[]>(jsonString);

            foreach (TheatreImportDto theatreDto in theatreDtos)
            {
                if (!IsValid(theatreDto))
                {
                    messageBuilder.AppendLine(ErrorMessage);
                    continue;
                }

                var theatre = new Theatre
                {
                    Name = theatreDto.Name,
                    NumberOfHalls = theatreDto.NumberOfHalls,
                    Director = theatreDto.Director,
                };

                foreach (TicketImportDto ticketDto in theatreDto.Tickets)
                {
                    Play play = context.Plays.FirstOrDefault(p => p.Id == ticketDto.PlayId);

                    if (!IsValid(ticketDto) || play == null)
                    {
                        messageBuilder.AppendLine(ErrorMessage);
                        continue;
                    }

                    var ticket = new Ticket
                    {
                        Price = ticketDto.Price,
                        RowNumber = ticketDto.RowNumber,
                        PlayId = ticketDto.PlayId,
                    };

                    theatre.Tickets.Add(ticket);
                }

                messageBuilder.AppendLine(
                    string.Format(SuccessfulImportTheatre, theatre.Name, theatre.Tickets.Count));

                context.Theatres.Add(theatre);
                context.SaveChanges();
            }

            return messageBuilder.ToString().TrimEnd();
        }

        private static bool IsValid(object obj)
        {
            var validator = new ValidationContext(obj);
            var validationRes = new List<ValidationResult>();

            var result = Validator.TryValidateObject(obj, validator, validationRes, true);
            return result;
        }
    }
}
