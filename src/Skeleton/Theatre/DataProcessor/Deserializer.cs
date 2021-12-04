namespace Theatre.DataProcessor
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Globalization;
    using System.IO;
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

                messageBuilder.AppendLine(string.Format(SuccessfulImportPlay, play.Title, play.Genre, play.Rating));
                context.Add(play);
                context.SaveChanges();
            }

            return messageBuilder.ToString().TrimEnd();
        }

        public static string ImportCasts(TheatreContext context, string xmlString)
        {
            throw new NotImplementedException();
        }

        public static string ImportTtheatersTickets(TheatreContext context, string jsonString)
        {
            throw new NotImplementedException();
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
