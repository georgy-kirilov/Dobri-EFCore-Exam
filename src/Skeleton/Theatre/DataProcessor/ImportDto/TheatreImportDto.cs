namespace Theatre.DataProcessor.ImportDto
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    using static Theatre.Validation.ValidationConstraints.Theatre;

    public class TheatreImportDto
    {
        [Required]
        [StringLength(NameMaxLength, MinimumLength = NameMinLength)]
        public string Name { get; set; }

        [Required]
        [Range(MaxNumberOfHalls, MaxNumberOfHalls)]
        public sbyte NumberOfHalls { get; set; }

        [Required]
        [StringLength(DirectorMaxLength, MinimumLength = DirectorMinLength)]
        public string Director { get; set; }

        public IEnumerable<TicketImportDto> Tickets { get; set; }
    }
}
